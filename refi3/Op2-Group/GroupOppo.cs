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
    public partial class GroupCommand : IExternalCommand
    {
        public bool EvaConnector(List<Element> listRefelement)
        {
            MEPCurve mepCurveOri = listRefelement[0] as MEPCurve;
            ConnectorSet connectorSet = mepCurveOri.ConnectorManager.Connectors;
            foreach(Connector con in connectorSet)
            {
                if (GetConnectorRef(con).Owner.Id == listRefelement[2].Id)
                {
                    return true;
                    break;
                }         
            }
            return false;
        }
        public void GroupOppo(Document doc, UIDocument uidoc, List<Element> listRefelement)
        {
            // Tham chiếu đến elemnt đã chọn
            Duct duct = listRefelement[0] as Duct;
            ElementId ductTypeId = duct.DuctType.Id;
            ElementId ductLevelId = duct.LookupParameter("Reference Level").AsElementId();
            DuctType ductType = duct.DuctType;
            ElementId systemId = duct.MEPSystem.GetTypeId();
            var element0 = listRefelement[0];
            var element1 = listRefelement[1];
            foreach (Element e in listRefelement)
            {
                if (e != listRefelement[1] && e != listRefelement[0]
                    && e != listRefelement[2] && e != listRefelement[listRefelement.Count - 1])
                {
                    doc.Delete(e.Id);
                }
            }
            Connector connector0 = GetConnector(element0, listRefelement[listRefelement.Count - 1], false);
            Connector connector1 = GetConnector(element1, listRefelement[2], false);
            //Ghana
            Connector connectorref0 = GetConnectorRef(connector0);
            Connector connectorref1 = GetConnectorRef(connector1);
            Duct newDuct = null;
            if (connector1.IsConnected == true)
            {
                if (connector0.IsConnected == true)
                {
                    newDuct = Duct.Create(doc, ductTypeId, ductLevelId, connectorref1, connectorref0);
                }
                else
                {
                    newDuct = Duct.Create(doc, ductTypeId, ductLevelId, connectorref1, connector0.Origin);
                }
            }
            else
            {
                if (connector0.IsConnected == true)
                {
                    newDuct = Duct.Create(doc, ductTypeId, ductLevelId, connectorref0, connector1.Origin);
                }
                else
                {
                    newDuct = Duct.Create(doc, systemId, ductTypeId, ductLevelId, connector1.Origin, connector0.Origin);
                }
            }
            Parameter paraH = newDuct.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM);
            paraH.Set(duct.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM).AsDouble());
            Parameter paraW = newDuct.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM);
            paraW.Set(duct.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM).AsDouble());

            foreach (Element e in listRefelement)
            {
                if (e == listRefelement[1] || e == listRefelement[0]
                    || e == listRefelement[2] || e == listRefelement[listRefelement.Count - 1])
                {
                    doc.Delete(e.Id);
                }
            }
        }

    }
}
