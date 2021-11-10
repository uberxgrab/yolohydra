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
    [TransactionAttribute(TransactionMode.Manual)]
    public partial class GroupCommand : IExternalCommand
    {
        public static Connector GetConnectorRef(Connector conDuct)
        {
            ElementId idDuct = conDuct.Owner.Id;
            ConnectorSet listref = conDuct.AllRefs;
            Connector confinal = null;
            foreach(Connector con in listref)
            {
                if(con.Owner.Id != idDuct)
                {
                    confinal = con;
                    break;
                }
            }
            return confinal;
        }
        public Element GetElementFromInt(int intId, Document doc)
        {
            ElementId eId = new ElementId(intId);
            var element = doc.GetElement(eId);
            return element;
        }
        public Connector GetConnector(Element element1, Element element2, Boolean boolean)
        {
            Duct ductElement = element1 as Duct;
            ConnectorSet connectorsetDuct = ductElement.ConnectorManager.Connectors;
            FamilyInstance filtingElement = element2 as FamilyInstance;
            ConnectorSet connectorsetFi = filtingElement.MEPModel.ConnectorManager.Connectors;
            Connector connectorFi = null;
            foreach (Connector co in connectorsetFi)
            {
                if (co.IsConnected == true)
                {
                    connectorFi = co;
                }
            }
            Connector connectorDuct = null;
            foreach (Connector co in connectorsetDuct)
            {
                var conref = GetConnectorRef(connectorFi); 
                if(co.Id != conref.Id)
                {
                    connectorDuct = co;
                    break;                    
                }
                //if (co.Origin.IsAlmostEqualTo(connectorFi.Origin) == boolean)
                //{
                //    connectorDuct = co;
                //}
            }
            return connectorDuct;
        }
        public void Group(Document doc, UIDocument uidoc, List<Element> listRefelement)
        {
            // Tham chiếu đến elemnt đã chọn
            Duct duct = listRefelement[0] as Duct;
            ElementId ductTypeId = duct.DuctType.Id;
            ElementId ductLevelId = duct.LookupParameter("Reference Level").AsElementId();
            DuctType ductType = duct.DuctType;
            ElementId systemId = duct.MEPSystem.GetTypeId();
            var element0 = listRefelement[0];
            var element1 = listRefelement[listRefelement.Count-2];
            foreach (Element e in listRefelement)
            {
                if (e != listRefelement[listRefelement.Count - 2] && e != listRefelement[0]
                    && e != listRefelement[2] && e != listRefelement[listRefelement.Count - 1])
                {
                    doc.Delete(e.Id);
                }
            }
            Connector connector0 = GetConnector(element0, listRefelement[2], false);
            Connector connector1 = GetConnector(element1, listRefelement[listRefelement.Count - 1], false);
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
                if (e == listRefelement[2] || e == listRefelement[listRefelement.Count - 1]
                    || e == listRefelement[listRefelement.Count - 2] || e == listRefelement[0])
                {
                    doc.Delete(e.Id);
                }
            }
        }
    }
}
