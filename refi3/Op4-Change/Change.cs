//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autodesk.Revit.DB;
//using Autodesk.Revit.DB.Mechanical;
//using Autodesk.Revit.UI;
//using Autodesk.Revit.Attributes;

//namespace refi3
//{
//    public partial class CommandChange : IExternalCommand
//    {
//        public void ChangeType(Document doc, FamilyInstance fillting, string name1, string name2)
//        {
//            FamilySymbol familySymbol = fillting.Symbol;
//            if(familySymbol.FamilyName == name1)
//            {
//                foreach (ElementId famId in familySymbol.GetSimilarTypes())
//                {
//                    var fam = doc.GetElement(famId) as ElementType;
//                    if (fam.FamilyName == name2)
//                    {
//                        var familySymbol1 = doc.GetElement(famId) as FamilySymbol;
//                        using (Transaction trans = new Transaction(doc, "change"))
//                        {
//                            trans.Start();
//                            fillting.Symbol = familySymbol1;
//                            trans.Commit();
//                        }
//                        break;
//                    }
//                }
//            }
//            else
//            {
//                foreach (ElementId famId in familySymbol.GetSimilarTypes())
//                {
//                    var fam = doc.GetElement(famId) as ElementType;
//                    if (fam.FamilyName == name1)
//                    {
//                        var familySymbol1 = doc.GetElement(famId) as FamilySymbol;
//                        using (Transaction trans = new Transaction(doc, "change"))
//                        {
//                            trans.Start();
//                            fillting.Symbol = familySymbol1;
//                            trans.Commit();
//                        }
//                        break;
//                    }
//                }
//            }
//        }
//        public void Change(Document doc, UIDocument uidoc, IList<Reference> listReference)
//        {
//            foreach (Reference reference in listReference)
//            {
//                try
//                {
//                    FamilyInstance fillting = doc.GetElement(reference.ElementId) as FamilyInstance;
//                    FamilySymbol familySymbol = fillting.Symbol;
//                    MechanicalFitting mechanicalFitting = fillting.MEPModel as MechanicalFitting;
//                    switch (mechanicalFitting.PartType)
//                    {
//                        case PartType.Elbow:
//                            ChangeType(doc, fillting, "01.REC Elbow(C Flanged)", "02.REC Elbow(TDC Flanged)");
//                            break;
//                        case PartType.TapPerpendicular:
//                            ChangeType(doc, fillting, "02.REC Tap -REC(TDC Flanged)", "01.REC Tap -REC(C Flanged)");
//                            break;
//                        case PartType.Transition:
//                            if (familySymbol.FamilyName == "02.REC Transition REC(TDC Flanged)" || 
//                                familySymbol.FamilyName == "01.REC Transition REC(C Flanged)")
//                            {
//                                ChangeType(doc, fillting, "02.REC Transition REC(TDC Flanged)", "01.REC Transition REC(C Flanged)");
//                            }
//                            else
//                            {
//                                ChangeType(doc, fillting, "02.REC Transistion - ROD(TDC Flanged)", "01.REC_Transition ROD(C Flanged)");
//                            }
//                            break;
//                        case PartType.Union:
//                            ChangeType(doc, fillting, "02.REC Union (TDC Flanged)", "01.REC_Union(C Flanged)");
//                            break;
//                        case PartType.Cap:
//                            ChangeType(doc, fillting, "02.REC_Cap End(TDC Flanged)", "01.REC_Cap End(C Flanged)");
//                            break;
//                    }
//                }
//                catch (Exception e)
//                {
//                }
//            }
//        }
//        public void Change(Document doc, UIDocument uidoc, Reference reference)
//        {
//                try
//                {
//                    FamilyInstance fillting = doc.GetElement(reference.ElementId) as FamilyInstance;
//                    FamilySymbol familySymbol = fillting.Symbol;
//                    MechanicalFitting mechanicalFitting = fillting.MEPModel as MechanicalFitting;
//                    switch (mechanicalFitting.PartType)
//                    {
//                        case PartType.Elbow:
//                            ChangeType(doc, fillting, "01.REC Elbow(C Flanged)", "02.REC Elbow(TDC Flanged)");
//                            break;
//                        case PartType.TapPerpendicular:
//                            ChangeType(doc, fillting, "02.REC Tap -REC(TDC Flanged)", "01.REC Tap -REC(C Flanged)");
//                            break;
//                        case PartType.Transition:
//                            if (familySymbol.FamilyName == "02.REC Transition REC(TDC Flanged)" ||
//                                familySymbol.FamilyName == "01.REC Transition REC(C Flanged)")
//                            {
//                                ChangeType(doc, fillting, "02.REC Transition REC(TDC Flanged)", "01.REC Transition REC(C Flanged)");
//                            }
//                            else
//                            {
//                                ChangeType(doc, fillting, "02.REC Transistion - ROD(TDC Flanged)", "01.REC_Transition ROD(C Flanged)");
//                            }
//                            break;
//                        case PartType.Union:
//                            ChangeType(doc, fillting, "02.REC Union (TDC Flanged)", "01.REC_Union(C Flanged)");
//                            break;
//                        case PartType.Cap:
//                            ChangeType(doc, fillting, "02.REC_Cap End(TDC Flanged)", "01.REC_Cap End(C Flanged)");
//                            break;
//                    }
//                }
//                catch (Exception e)
//                {
//                }
            
//        }
//    }
//}
