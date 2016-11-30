using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.SqlClient;


namespace ETMFDemo
{
  public  static class ObjectContextExtensions
    {

      public static List<TType> ReadObjectList<TType>(this MPMSEXTEntities context, string queryString, string fieldsToskip = null) where TType : new()
      {
          var sqlConnect =(SqlConnection) context.Database.Connection;
          SqlCommand cmd = new SqlCommand(queryString);
          cmd.Connection = sqlConnect;
          List<TType> returnResult = null;
          try
          {
              sqlConnect.Open();
              var data = cmd.ExecuteReader();
              if (data.HasRows)
              {
                  returnResult = data.DataReaderToObjectList<TType>(fieldsToskip);
              }
              else
              {
                    returnResult = new List<TType>();
              }
          }
          catch (Exception ex)
          {
            
          }
          finally
          {
              sqlConnect.Close();
          }
          return returnResult;
      }
    }
}
