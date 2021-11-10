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
    public class Filter 
    {
        public static void FilterDuct(Document doc, UIDocument uidoc)
        {
            FilteredElementCollector collection = new FilteredElementCollector(doc);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
            
           
            // lấy danh sach ekemnt đã lọc
            DuctType ductType = collection.WherePasses(filter).WhereElementIsElementType()
                    .Cast<DuctType>()
                    .First(x => x.Name == "Tap_C Flanged" && x.FamilyName == "Rectangular Duct");
            IList<ElementId> listDuct = ductType.GetDependentElements(filter);

            foreach (ElementId e in listDuct)
            {
                try
                {
                    //doc.Delete(e);
                    SplitFunc.SplitOpposite(doc, uidoc, doc.GetElement(e));
                }
                catch (Exception exception)
                {

                }
            }
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }

    }
}