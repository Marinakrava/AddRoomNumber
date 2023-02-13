using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddRoomNumber
{
    [Transaction(TransactionMode.Manual)]

    public class RoomNumber : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            int roomNumber = 0;
           
            ElementCategoryFilter roomCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);

            // 
            ElementId level1Id = new FilteredElementCollector(doc)
             .OfClass(typeof(Level))
             .Where(x => x.Name.Equals("Level 1"))
             .FirstOrDefault().Id;

            // фильтр помещений 2 этажа
            ElementId level2Id = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .Where(x => x.Name.Equals("Level 2"))
            .FirstOrDefault().Id;

            ElementLevelFilter level1Filter = new ElementLevelFilter(level1Id);
            LogicalAndFilter roomsFilter1 = new LogicalAndFilter(roomCategoryFilter, level1Filter);
            ElementLevelFilter level2Filter = new ElementLevelFilter(level2Id);
            LogicalAndFilter roomsFilter2 = new LogicalAndFilter(roomCategoryFilter, level2Filter);

            // список помещений 1 этажа
            List<SpatialElement> rooms1 = new FilteredElementCollector(doc)
              .OfCategory(BuiltInCategory.OST_Rooms)
              .OfClass(typeof(SpatialElement))
              .WherePasses(roomsFilter1)
              .Cast<SpatialElement>()
              .ToList();

            // нумеруем помещения 1 этажа
            Transaction ts1 = new Transaction(doc);
            ts1.Start("Нумерация помещений на 1 этаже");

            foreach (SpatialElement r1 in rooms1)
            {
                roomNumber++;
                string calcRoomNumber;
                calcRoomNumber = $"{roomNumber}";
                room.get_Parameter(BuiltInParameter.ROOM_NUMBER).Set(calcRoomNumber);
            }


            ts1.Commit();
        }
        return Result.Succeeded;
                                                  
    }
}
