using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;


namespace refi3
{ 
    public partial class SplitFunc 
    {
        public static Connector Getconplaceunion(XYZ evaluatedPoint, ElementId ductid, Document doc)
        {
            var mEPCurve0 = doc.GetElement(ductid) as MEPCurve;
            var lisstconector0 = mEPCurve0.ConnectorManager.Connectors;
            Connector connector = null;
            foreach (Connector connector1 in lisstconector0)
            {
                if (connector1.Origin.IsAlmostEqualTo(evaluatedPoint) == true )
                {
                    connector = connector1;
                    break;
                }
            }
            return connector;
        }
        public static XYZ Getevaluate(Curve c, double ftLength, Document doc)
        {
            double param1 = c.GetEndParameter(0);
            double param2 = c.GetEndParameter(1);
            double paramCalc = param2 - ftLength;
            XYZ evaluatedPoint = null;
            if (c.IsInside(paramCalc))
            {
                double normParam = c.ComputeNormalizedParameter(paramCalc);
                evaluatedPoint = c.Evaluate(normParam, true);
            }
            return evaluatedPoint;
        }
        public static void SplitOpposite(Document doc, UIDocument uidoc, Element element)
        {
            try
            {
                LocationCurve lc = element.Location as LocationCurve;
                var c = lc.Curve;
                double mmLength = double.Parse(UI.item3.Value.ToString()) - 3;
                double ftLength = UnitUtils.Convert(mmLength, UnitTypeId.Millimeters, UnitTypeId.Feet);
                ElementId prelDuctid = null;
                double a = Math.Truncate((c.GetEndParameter(1) - c.GetEndParameter(0)) / ftLength);
                for (int i = 0; i < a; i++)
                {
                    if (i == 0)
                    {
                        var evaluatedPoint = Getevaluate(c, ftLength, doc);
                        ElementId ductid1 = MechanicalUtils.BreakCurve(doc, element.Id, evaluatedPoint);
                        var connector0 = Getconplaceunion(evaluatedPoint, element.Id, doc);
                        var connector1 = Getconplaceunion(evaluatedPoint, ductid1, doc);
                        doc.Create.NewUnionFitting(connector1, connector0);
                        prelDuctid = ductid1;
                    }
                    else
                    {
                        try
                        {
                            element = doc.GetElement(prelDuctid);
                            //lấy thông tin element
                            lc = element.Location as LocationCurve;
                            c = lc.Curve;
                            var evaluatedPoint = Getevaluate(c, ftLength, doc);
                            ElementId ductid = MechanicalUtils.BreakCurve(doc, prelDuctid, evaluatedPoint);
                            var connector0 = Getconplaceunion(evaluatedPoint, ductid, doc);
                            var connector1 = Getconplaceunion(evaluatedPoint, prelDuctid, doc);
                            doc.Create.NewUnionFitting(connector1, connector0);
                            prelDuctid = ductid;
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
            
        }
    }
}