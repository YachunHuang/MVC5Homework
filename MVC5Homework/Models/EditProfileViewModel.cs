using System.ComponentModel.DataAnnotations;

namespace MVC5Homework.Models
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        //[StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        //[Required]
        //public string 帳號 { get; set; }

        public string 密碼 { get; set; }

        //[StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        //[Required]
        //public string 客戶名稱 { get; set; }

        //[StringLength(8, ErrorMessage = "欄位長度不得大於 8 個字元")]
        //[Required]
        //public string 統一編號 { get; set; }

        public string 電話 { get; set; }

        public string 傳真 { get; set; }

        public string 地址 { get; set; }

        public string Email { get; set; }
    }
}