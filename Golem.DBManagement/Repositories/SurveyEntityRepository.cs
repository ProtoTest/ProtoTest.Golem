using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;
using Golem.DBManagement.Entities;

namespace Golem.DBManagement.Repositories
{
    public static class SurveyEntityRepository
    {
        private static List<SurveyEntity> _surveyEntities;
        private static SurveyEntity _surveyEntity;

        public static List<SurveyEntity> GetSurveyInfoBySurveySubmissionId(long surveyId)
        {
            _surveyEntities = new List<SurveyEntity>();

            using (IDBManager dbManager = DataAccess.GetSqlConnection())
            {
                try
                {
                    string commandText = string.Format(DataAccess.GetSqlQuery("SurveySql.xml", "GetSurveyUsingSurveySubmissionId"), surveyId);
                    dbManager.ExecuteReader(CommandType.Text, commandText + surveyId);

                    while (dbManager.DataReader.Read())
                    {
                        _surveyEntities.Add(MapSurveySubmissionTableToSurveyEntityObject(dbManager.DataReader));
                    }

                    return _surveyEntities;
                }

                catch (Exception e)
                {
                    throw new DataException(e.Message);
                }

                finally
                { 
                
                }
            }
        }


        public static int GetProviderIdBySurveySubmissionId(long surveyId)
        {
            _surveyEntity = new SurveyEntity();
            int providerId = 0;
            
            using (IDBManager dbManager = DataAccess.GetSqlConnection())
            {
                try
                {
                    string commandText = string.Format(DataAccess.GetSqlQuery("SurveySql.xml", "GetSurveyUsingSurveySubmissionId"), surveyId);
                    dbManager.ExecuteReader(CommandType.Text, commandText + surveyId);

                    while (dbManager.DataReader.Read())
                    {
                       return (providerId = Convert.ToInt32(dbManager.DataReader["ProviderId"]));
                    }

                    return 0;

                }

                catch (Exception e)
                {
                    throw new DataException(e.Message);
                }
                                
                finally 
                {
                
                }
            }

        }

        public static string GetBlobDataById(int id)
        {
            //_surveyEntity = new SurveyEntity();

            using (IDBManager dbManager = DataAccess.GetSqlConnection())
            {
                try
                {
                    string commandText = string.Format(DataAccess.GetSqlQuery("SurveySql.xml", "GetBlobDataUsingId"), id);
                    byte[] blob = (byte[])dbManager.ExecuteScalar(CommandType.Text,commandText);
                    var results = System.Text.Encoding.Default.GetString(blob);


                    while (dbManager.DataReader.Read())
                    {
                        return results;
                    }

                    return null;

                }

                catch (Exception e)
                {
                    throw new DataException(e.Message);
                }

                finally
                {

                }
            }

        }


          

        public static SurveyEntity MapSurveySubmissionTableToSurveyEntityObject(IDataReader dataReader)
        {
            _surveyEntity = new SurveyEntity();

            if (!(System.Convert.IsDBNull(dataReader["SurveySubmissionID"])))
            {
                _surveyEntity.SurveySubmissionID = Convert.ToInt32(dataReader["SurveySubmissionID"]);
            }
            else
            {
                throw new DataException("SurveySubmissionID cannot be null");
            }

            if (!(System.Convert.IsDBNull(dataReader["ProviderID"])))
            {
                _surveyEntity.ProviderID = Convert.ToInt32(dataReader["ProviderID"]);
            }
            else
            {
                throw new DataException("ProviderID cannot be null");
            }

            if (!(System.Convert.IsDBNull(dataReader["UserEmail"])))
            {
                _surveyEntity.UserEmail = dataReader["UserEmail"].ToString();
            }
            else
            {
                throw new DataException("UserEmail cannot be null");
            }

            if (!(System.Convert.IsDBNull(dataReader["SubmitDate"])))
            {
                _surveyEntity.SubmitDate = Convert.ToDateTime(dataReader["SubmitDate"]);
            }
            else
            {
                throw new DataException("SubmitDate cannot be null");
            }

            return _surveyEntity;
        }
    }
}
