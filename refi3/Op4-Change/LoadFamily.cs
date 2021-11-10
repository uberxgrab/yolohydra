using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.IO;

namespace refi3
{
    public partial class CommandChange : IExternalCommand
    {
        public static string statusButton;
        public static List<FamilySymbol> listSym;
        public static Family tapFam;
        public FamilySymbol AddRouting(Document doc, UIDocument uidoc, string path)
        {
            using (Transaction transaction = new Transaction(doc, "Routing Preference"))
            {
                transaction.Start();
                FamilySymbol symbol = null;
                doc.LoadFamily(path, new FamilyOption() ,out tapFam);
                ISet<ElementId> familySymbolIds = tapFam.GetFamilySymbolIds();
                ElementId id = familySymbolIds.ElementAt(0);

                symbol = tapFam.Document.GetElement(id) as FamilySymbol;

                if ((!symbol.IsActive) && (symbol != null))
                {
                    symbol.Activate();
                    doc.Regenerate();
                }
                transaction.Commit();
                return symbol;
            }
        }
        public static FileInfo[] GetFileFam(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] listFile = d.GetFiles();
            List<string> filePath = new List<string>();
            return listFile;
        }
        public static string GetNameWithoutExten(string name, string extension)
        {
            name.Replace(extension, "");
            var nameNoEx = name;
            return nameNoEx;
        }
        public static FamilySymbol CheckFam(Document doc, FamilyInstance fillting, string name2)
        {
            FamilySymbol familySymbol = fillting.Symbol;
            FamilySymbol symfinal = null;
            foreach (ElementId famId in familySymbol.GetSimilarTypes())
            {
                var fam = doc.GetElement(famId) as ElementType;
                if (fam.FamilyName == name2)
                {
                    symfinal = doc.GetElement(famId) as FamilySymbol;
                    break;
                }
            }
            return symfinal;
        }
        public List<FamilySymbol> GetFamilySymbols(Document doc, UIDocument uidoc, FamilyInstance fillting)
        {
            FileInfo[] listFile  = GetFileFam(@"D:\Family Duct Fitting\"+UI.comboBoxFam.CurrentButton.ItemText);
            List<FamilySymbol> listSym = new List<FamilySymbol>(6);
            for (int i = 0; i <= 5; ++i)
            {
                listSym.Add(null);
            }
            FamilySymbol symbol = null;
            foreach (FileInfo file in listFile)
            {
                var symCheck = CheckFam(doc, fillting, Path.GetFileNameWithoutExtension(file.FullName) /*GetNameWithoutExten(file.Name, file.Extension)*/);
                if(symCheck == null)
                {
                    symbol = AddRouting(doc, uidoc, file.FullName);
                }
                else
                {
                    symbol = symCheck;
                }
                switch (symbol.Family.get_Parameter(BuiltInParameter.FAMILY_CONTENT_PART_TYPE).AsValueString())
                {
                    case "Elbow":
                        listSym.Insert(0, symbol);
                        break;
                    case "Tap - Perpendicular":
                        listSym.Insert(1, symbol);
                        break;
                    case "Transition":
                        if(symbol.Family.get_Parameter(BuiltInParameter.FAMILY_ROUNDCONNECTOR_DIMENSIONTYPE)
                            .AsInteger() == 1)
                        {
                            listSym.Insert(2, symbol);
                            break;
                        }
                        else
                        {
                            listSym.Insert(3, symbol);
                            break;
                        }
                    case "Union":
                        listSym.Insert(4, symbol);
                        break;
                    case "Cap":
                        listSym.Insert(5, symbol);
                        break;
                }
            }
            return listSym;
        }
    }
}
