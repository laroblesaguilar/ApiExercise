﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs
{
    public interface IDynamoDbExample
    {
        void CreateDynamoDbTableAsync();
        Task Insert();
        Task InsertObjectPersistenceModel();
    }
}
