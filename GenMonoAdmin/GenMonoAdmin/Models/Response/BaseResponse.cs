using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GenMonoAdmin.Models.Response
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }

        public int Code { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Object { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<T> Objects { get; set; }
    }
}
