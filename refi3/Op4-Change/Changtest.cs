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
        public void ChangeType(Document doc, FamilyInstance fillting, FamilySymbol familySymbol)
        {
            using (Transaction trans = new Transaction(doc, "change"))
            {
                trans.Start();
                fillting.Symbol = familySymbol;
                trans.Commit();
            }
        }
        public void Change(Document doc, UIDocument uidoc, Reference reference)
        {
            try
            {
                FamilyInstance fillting = doc.GetElement(reference.ElementId) as FamilyInstance;
                FamilySymbol familySymbol = fillting.Symbol;
                MechanicalFitting mechanicalFitting = fillting.MEPModel as MechanicalFitting;
                List<FamilySymbol> familySymbols = GetFamilySymbols(doc, uidoc, fillting);
                switch (mechanicalFitting.PartType)
                {
                    case PartType.Elbow:
                        ChangeType(doc, fillting, familySymbols[0]);
                        break;
                    case PartType.TapPerpendicular:
                        ChangeType(doc, fillting, familySymbols[1]);
                        break;
                    case PartType.Transition:
                        if (familySymbol.Family.get_Parameter(BuiltInParameter.FAMILY_ROUNDCONNECTOR_DIMENSIONTYPE)
                            .AsInteger() == 1)
                        {
                            ChangeType(doc, fillting, familySymbols[2]);
                        }
                        else
                        {
                            ChangeType(doc, fillting, familySymbols[3]);
                        }
                        break;
                    case PartType.Union:
                        ChangeType(doc, fillting, familySymbols[4]);
                        break;
                    case PartType.Cap:
                        ChangeType(doc, fillting, familySymbols[5]);
                        break;
                }
        }
            catch (Exception e)
            {
            }
}
    }
}
