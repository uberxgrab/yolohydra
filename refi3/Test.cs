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
    class Test : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("get", UI.comboBoxFam.CurrentButton.ItemText);
            return Result.Succeeded;
        }
    }
}
