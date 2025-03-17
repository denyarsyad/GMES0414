using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JPlatform.Client.JBaseForm6;
using System.Data;
using System.Data.SqlClient;

namespace CSI.MES.P.DAO
{
    public class SP_GMES0414 : BaseProcClass
    {
        public SP_GMES0414(string type = "Q")
        {
            // Modify Code : Procedure Name
            if (type == "Q")
            {
                _ProcName = "SP_GMES0414_Q";
                ParamAdd();
            }
            else if (type == "S")
            {
                _ProcName = "SP_GMES0414_S";
                ParamAddInsert();
            }
        }

        private void ParamAdd()
        {
            _ParamInfo.Add(new ParamInfo("@V_P_TYPES", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FROM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_TO", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_STATUS", "Varchar", 500, "Input", typeof(System.String)));
        }

        public DataTable SetParamData(DataTable dataTable,
                              System.String V_P_TYPE = "",
                              System.String V_P_FROM = "",
                              System.String V_P_TO = "",
                              System.String V_P_STATUS = "")
        {
            if (dataTable == null)
            {
                dataTable = new DataTable(_ProcName);
                foreach (ParamInfo pi in _ParamInfo)
                {
                    dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
                }
            }
            // Modify Code : Procedure Parameter
            object[] objData = new object[] { V_P_TYPE, V_P_FROM, V_P_TO, V_P_STATUS };
            dataTable.Rows.Add(objData);
            return dataTable;
        }

        //SAVE
        private void ParamAddInsert()
        {
            _ParamInfo.Add(new ParamInfo("@V_P_ACTION", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_DATE", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_TIME", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_DIV", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PLANT_CD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_USER_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_USER_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_USER_EMPID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_USER_DEPT_CD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_USER_DEPT_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_TYPE_CD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_ACTIVITY_CD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_PLACE_CD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_PLACE_DESC", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_PRIORITY", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_EMP_QTY", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_USED_DESC", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_LEADER_ID1", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_LEADER_ID2", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_APPROV_YN", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_APPROV_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PLAN_START_TIME", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PLAN_END_TIME", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PLAN_DURATION_HH", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_ITEM_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_ITEM_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_BY_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_BY_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_YN", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_PREPARED_MEMO", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_USER_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_USER_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_BY_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_BY_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_YN", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_DESC", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_ATTACHMENT_TEXT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_ATTACHMENT_EXT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_ATTACHMENT_BLOB", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_ONGOING_MEMO", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CANCEL_BY_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CANCEL_BY_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CANCEL_YN", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CANCEL_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CANCEL_MEMO", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_USER_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_USER_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_BY_ID", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_BY_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_YN", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_REMARK", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_ATTACHMENT_NM", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_ATTACHMENT_EXT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_FINISH_ATTACHMENT_BLOB", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_RENTAL_STATUS", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_DATA_MEMO", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_EXTRA1_FLD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_EXTRA2_FLD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_EXTRA3_FLD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_EXTRA4_FLD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_EXTRA5_FLD", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CREATOR", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CREATE_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CREATE_PC", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_UPDATER", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_UPDATE_DT", "Varchar", 500, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_UPDATE_PC", "Varchar", 500, "Input", typeof(System.String)));
        }

        public DataTable SetParamDataInsert(DataTable dataTable,
                                            System.String V_P_ACTION = "",
                                            System.String V_P_RENTAL_DATE = "",
                                            System.String V_P_RENTAL_TIME = "",
                                            System.String V_P_RENTAL_DIV = "",
                                            System.String V_P_PLANT_CD = "",
                                            System.String V_P_USER_ID = "",
                                            System.String V_P_USER_NM = "",
                                            System.String V_P_USER_EMPID = "",
                                            System.String V_P_USER_DEPT_CD = "",
                                            System.String V_P_USER_DEPT_NM = "",
                                            System.String V_P_RENTAL_TYPE_CD = "",
                                            System.String V_P_RENTAL_ACTIVITY_CD = "",
                                            System.String V_P_RENTAL_PLACE_CD = "",
                                            System.String V_P_RENTAL_PLACE_DESC = "",
                                            System.String V_P_RENTAL_PRIORITY = "",
                                            System.String V_P_RENTAL_EMP_QTY = "",
                                            System.String V_P_RENTAL_USED_DESC = "",
                                            System.String V_P_RENTAL_LEADER_ID1 = "",
                                            System.String V_P_RENTAL_LEADER_ID2 = "",
                                            System.String V_P_RENTAL_APPROV_YN = "",
                                            System.String V_P_RENTAL_APPROV_DT = "",
                                            System.String V_P_PLAN_START_TIME = "",
                                            System.String V_P_PLAN_END_TIME = "",
                                            System.String V_P_PLAN_DURATION_HH = "",
                                            System.String V_P_PREPARED_ITEM_ID = "",
                                            System.String V_P_PREPARED_ITEM_NM = "",
                                            System.String V_P_PREPARED_BY_ID = "",
                                            System.String V_P_PREPARED_BY_NM = "",
                                            System.String V_P_PREPARED_YN = "",
                                            System.String V_P_PREPARED_DT = "",
                                            System.String V_P_PREPARED_MEMO = "",
                                            System.String V_P_ONGOING_USER_ID = "",
                                            System.String V_P_ONGOING_USER_NM = "",
                                            System.String V_P_ONGOING_BY_ID = "",
                                            System.String V_P_ONGOING_BY_NM = "",
                                            System.String V_P_ONGOING_DT = "",
                                            System.String V_P_ONGOING_YN = "",
                                            System.String V_P_ONGOING_DESC = "",
                                            System.String V_P_ONGOING_ATTACHMENT_TEXT = "",
                                            System.String V_P_ONGOING_ATTACHMENT_EXT = "",
                                            System.String V_P_ONGOING_ATTACHMENT_BLOB = "",
                                            System.String V_P_ONGOING_MEMO = "",
                                            System.String V_P_CANCEL_BY_ID = "",
                                            System.String V_P_CANCEL_BY_NM = "",
                                            System.String V_P_CANCEL_YN = "",
                                            System.String V_P_CANCEL_DT = "",
                                            System.String V_P_CANCEL_MEMO = "",
                                            System.String V_P_FINISH_USER_ID = "",
                                            System.String V_P_FINISH_USER_NM = "",
                                            System.String V_P_FINISH_BY_ID = "",
                                            System.String V_P_FINISH_BY_NM = "",
                                            System.String V_P_FINISH_DT = "",
                                            System.String V_P_FINISH_YN = "",
                                            System.String V_P_FINISH_REMARK = "",
                                            System.String V_P_FINISH_ATTACHMENT_NM = "",
                                            System.String V_P_FINISH_ATTACHMENT_EXT = "",
                                            System.String V_P_FINISH_ATTACHMENT_BLOB = "",
                                            System.String V_P_RENTAL_STATUS = "",
                                            System.String V_P_DATA_MEMO = "",
                                            System.String V_P_EXTRA1_FLD = "",
                                            System.String V_P_EXTRA2_FLD = "",
                                            System.String V_P_EXTRA3_FLD = "",
                                            System.String V_P_EXTRA4_FLD = "",
                                            System.String V_P_EXTRA5_FLD = "",
                                            System.String V_P_CREATOR = "",
                                            System.String V_P_CREATE_DT = "",
                                            System.String V_P_CREATE_PC = "",
                                            System.String V_P_UPDATER = "",
                                            System.String V_P_UPDATE_DT = "",
                                            System.String V_P_UPDATE_PC = ""
                                           )
        {
            if (dataTable == null)
            {
                dataTable = new DataTable(_ProcName);
                foreach (ParamInfo pi in _ParamInfo)
                {
                    dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
                }
            }
            // Modify Code : Procedure Parameter
            object[] objData = new object[] {
                                    V_P_ACTION,
                                    V_P_RENTAL_DATE,
                                    V_P_RENTAL_TIME,
                                    V_P_RENTAL_DIV,
                                    V_P_PLANT_CD,
                                    V_P_USER_ID,
                                    V_P_USER_NM,
                                    V_P_USER_EMPID,
                                    V_P_USER_DEPT_CD,
                                    V_P_USER_DEPT_NM,
                                    V_P_RENTAL_TYPE_CD,
                                    V_P_RENTAL_ACTIVITY_CD,
                                    V_P_RENTAL_PLACE_CD,
                                    V_P_RENTAL_PLACE_DESC,
                                    V_P_RENTAL_PRIORITY,
                                    V_P_RENTAL_EMP_QTY,
                                    V_P_RENTAL_USED_DESC,
                                    V_P_RENTAL_LEADER_ID1,
                                    V_P_RENTAL_LEADER_ID2,
                                    V_P_RENTAL_APPROV_YN,
                                    V_P_RENTAL_APPROV_DT,
                                    V_P_PLAN_START_TIME,
                                    V_P_PLAN_END_TIME,
                                    V_P_PLAN_DURATION_HH,
                                    V_P_PREPARED_ITEM_ID,
                                    V_P_PREPARED_ITEM_NM,
                                    V_P_PREPARED_BY_ID,
                                    V_P_PREPARED_BY_NM,
                                    V_P_PREPARED_YN,
                                    V_P_PREPARED_DT,
                                    V_P_PREPARED_MEMO,
                                    V_P_ONGOING_USER_ID,
                                    V_P_ONGOING_USER_NM,
                                    V_P_ONGOING_BY_ID,
                                    V_P_ONGOING_BY_NM,
                                    V_P_ONGOING_DT,
                                    V_P_ONGOING_YN,
                                    V_P_ONGOING_DESC,
                                    V_P_ONGOING_ATTACHMENT_TEXT,
                                    V_P_ONGOING_ATTACHMENT_EXT,
                                    V_P_ONGOING_ATTACHMENT_BLOB,
                                    V_P_ONGOING_MEMO,
                                    V_P_CANCEL_BY_ID,
                                    V_P_CANCEL_BY_NM,
                                    V_P_CANCEL_YN,
                                    V_P_CANCEL_DT,
                                    V_P_CANCEL_MEMO,
                                    V_P_FINISH_USER_ID,
                                    V_P_FINISH_USER_NM,
                                    V_P_FINISH_BY_ID,
                                    V_P_FINISH_BY_NM,
                                    V_P_FINISH_DT,
                                    V_P_FINISH_YN,
                                    V_P_FINISH_REMARK,
                                    V_P_FINISH_ATTACHMENT_NM,
                                    V_P_FINISH_ATTACHMENT_EXT,
                                    V_P_FINISH_ATTACHMENT_BLOB,
                                    V_P_RENTAL_STATUS,
                                    V_P_DATA_MEMO,
                                    V_P_EXTRA1_FLD,
                                    V_P_EXTRA2_FLD,
                                    V_P_EXTRA3_FLD,
                                    V_P_EXTRA4_FLD,
                                    V_P_EXTRA5_FLD,
                                    V_P_CREATOR,
                                    V_P_CREATE_DT,
                                    V_P_CREATE_PC,
                                    V_P_UPDATER,
                                    V_P_UPDATE_DT,
                                    V_P_UPDATE_PC
            };
            dataTable.Rows.Add(objData);
            return dataTable;
        }
    }
}
