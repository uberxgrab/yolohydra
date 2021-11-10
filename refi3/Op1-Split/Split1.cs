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
        public static XYZ GetevaluateFromOrigin(Curve c, double ftLength, Document doc)
        {
            double param1 = c.GetEndParameter(0);
            double param2 = c.GetEndParameter(1);
            double paramCalc = param1 + ftLength;
            XYZ evaluatedPoint = null;
            if (c.IsInside(paramCalc))
            {
                double normParam = c.ComputeNormalizedParameter(paramCalc);
                evaluatedPoint = c.Evaluate(normParam, true);
            }
            return evaluatedPoint;
        }
        public static void SplitFromOrigin(Document doc, UIDocument uidoc, Reference refelement)
        {
            try
            {
                Element element = doc.GetElement(refelement.ElementId);
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
                        var evaluatedPoint = GetevaluateFromOrigin(c, ftLength, doc);
                        ElementId ductid1 = MechanicalUtils.BreakCurve(doc, element.Id, evaluatedPoint);
                        var connector0 = Getconplaceunion(evaluatedPoint, element.Id, doc);
                        var connector1 = Getconplaceunion(evaluatedPoint, ductid1, doc);
                        doc.Create.NewUnionFitting(connector1, connector0);
                        prelDuctid = refelement.ElementId;
                    }
                    else
                    {
                        element = doc.GetElement(prelDuctid);
                        //lấy thông tin element
                        lc = element.Location as LocationCurve;
                        c = lc.Curve;
                        var evaluatedPoint = GetevaluateFromOrigin(c, ftLength, doc);
                        ElementId ductid = MechanicalUtils.BreakCurve(doc, prelDuctid, evaluatedPoint);
                        ////////TaskDialog.Show("Name", ductid1.ToString());
                        var connector0 = Getconplaceunion(evaluatedPoint, ductid, doc);
                        var connector1 = Getconplaceunion(evaluatedPoint, prelDuctid, doc);
                        doc.Create.NewUnionFitting(connector1, connector0);
                        prelDuctid = refelement.ElementId;
                    }
                }
            }
            catch(Exception e)
            {

            }
            
        }
    }
}
