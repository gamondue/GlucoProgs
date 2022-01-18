using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public partial class DL_FlatText : DataLayer
    {
        public  override void PurgeDatabase()
        {
            throw new NotImplementedException();
        }
        public  override long? SaveParameter(string FieldName, string FieldValue, int? Key = null)
        {
            throw new NotImplementedException();
        }
        public  override string RestoreParameter(string FieldName, int? Key = null)
        {
            throw new NotImplementedException();
        }
        //public override string? ToString()
        //{
        //    return base.ToString();
        //}
        #region Meals and Foods
        public  override List<Meal> ReadMeals(DateTime? initialTime, DateTime? finalTime)
        {
            throw new NotImplementedException();
        }
        public  override void SaveMeals(List<Meal> List)
        {
            throw new NotImplementedException();
        }
        public  override long? SaveOneMeal(Meal Meal)
        {
            throw new NotImplementedException();
        }
        public  override List<FoodInMeal> ReadFoodsInMeal(int? IdMeal)
        {
            throw new NotImplementedException();
        }
        public  override int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            throw new NotImplementedException();
        }
        public  override void SaveFoodsInMeal(List<FoodInMeal> List)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
