using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MVC5Homework.Models
{
    internal class 驗證手機格式Attribute : DataTypeAttribute
    {
        public 驗證手機格式Attribute() : base(@"^\d{4}-\d{6}$")
            //(DataType.Text)
        {

        }
        //TODO:驗證手機格式Attribute，驗證可以直接在base傳入即可。與下面的方法意思相同
        //public override bool IsValid(object data)
        //{
        //    if (data == null) { return true; }

        //    string str = (string)data;
        //    Regex regex = new Regex(@"^\d{4}-\d{6}$");

        //    return regex.IsMatch(str);
        //}
    }
}