using MongoDB.Bson.Serialization.Attributes;
using Extension;
using System.Text.Json.Serialization;

namespace DatabaseHelper.Identities
{
    public class User
    {
        [BsonId]
        [BsonRequired]
        public string UserID { get; set; }

        [BsonRequired]
        [BsonElement("FullName")]
        public string FullName { get; set; }

        [BsonRequired]
        [BsonElement("DateOfBirth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DateOfBirth { get; set; }

        [BsonRequired]
        [BsonElement("Gender")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Gender { get; set; }

        [BsonElement("AddressLine")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AddressLine { get; set; }

        [BsonElement("StateOrCity")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string StateOrCity { get; set; }

        [BsonElement("Country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Country { get; set; }

        [BsonElement("Email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Email { get; set; }

        [BsonElement("PhoneNo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string PhoneNo { get; set; }

        [BsonRequired]
        [BsonElement("HashPassword")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string HashPassword { get; set; }

        [BsonRequired]
        [BsonElement("isVerifiedEmail")]
        public string isVerifiedEmail { get; set; }

        [BsonRequired]
        [BsonElement("isVerifiedPhone")]
        public string isVerifiedPhone { get; set; }

        [BsonRequired]
        [BsonElement("isActived")]
        public string isActived { get; set; }

        public bool isValid()
        {
            if (this.UserID.IsNullOrEmpty()) {
                return false;
            }

            if (this.DateOfBirth.IsNullOrEmpty())
            {
                return false;
            }

            if (this.FullName.IsNullOrEmpty())
            {
                return false;
            }

            if (this.Gender.IsNullOrEmpty())
            {
                return false;
            }

            if (this.HashPassword.IsNullOrEmpty())
            {
                return false;
            }

            if (this.Email.IsNullOrEmpty() && this.PhoneNo.IsNullOrEmpty()) {
                return false;
            }

            if (this.isVerifiedEmail.IsNullOrEmpty())
            {
                return false;
            }

            if (this.isVerifiedPhone.IsNullOrEmpty())
            {
                return false;
            }

            if (this.isActived.IsNullOrEmpty())
            {
                return false;
            }

            return true; 
        }
    }
}
