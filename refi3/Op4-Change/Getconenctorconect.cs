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
    public partial class CommandChange : IExternalCommand
    {
        public Connector GetConnector(Connector connector)
        {
            Element element = connector.Owner;
            Connector confinal = null;
            try
            {
                FamilyInstance familyInstance = element as FamilyInstance;
                ConnectorSet listcon = familyInstance.MEPModel.ConnectorManager.Connectors;
                foreach (Connector con in listcon)
                {
                    if (con.Id != connector.Id)
                    {
                        confinal = con;
                    }
                }
                return confinal;

            }
            catch (Exception e)
            {
                MEPCurve mepCurve = element as MEPCurve;
                ConnectorSet listcon = mepCurve.ConnectorManager.Connectors;
                foreach (Connector con in listcon)
                {
                    if (con.Id != connector.Id)
                    {
                        confinal = con;
                    }
                }
                return confinal;

            }
        }
        public ElementId RecursionElement(Document doc, UIDocument uidoc, Connector connector)
        {
            if(connector.IsConnected == false)
            {
                TaskDialog.Show("task", "Done");
                return connector.Owner.Id;
            }
            else
            {
                Connector conref = GroupCommand.GetConnectorRef(connector);
                Connector connext = GetConnector(conref);
                doc.Delete(RecursionElement(doc, uidoc, connext));
                return connext.Owner.Id;
            }
        }
        public List<Element> Getconnectorconect(Document doc, UIDocument uidoc ,Reference reference)
        {
            Duct duct = doc.GetElement(reference.ElementId) as Duct;
            ConnectorSet listconduct = duct.ConnectorManager.Connectors;
            List<Element> listelement = new List<Element>();
            foreach (Connector con in listconduct)
            {
                if (con.IsConnected == true)
                {
                    using (Transaction trans = new Transaction(doc, "Test"))
                    {
                        trans.Start();
                        try
                        {
                            doc.Delete(RecursionElement(doc, uidoc, con));
                        }
                        catch (Exception e)
                        {

                        }
                        trans.Commit();
                    }
                }
                else
                {

                }
            }
            return listelement;
        }
    }
}
