﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using 反窃电.Models;

namespace 反窃电.DAL
{
    public class SqlHelper
    {
        private static SqlHelper sqlHelper = new SqlHelper();
        public static SqlHelper Helper() { return sqlHelper; }

        private SqlConnection conn = null;
        private SqlCommand cmd = null;

        public SqlHelper()
        {
            //建立SQL链接          
            string connstr = ConfigurationManager.ConnectionStrings["FanQieDian"].ConnectionString;
            conn = new SqlConnection(connstr);
        }

        private SqlConnection getconn()
        {//打开链接            
            if(conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;

        }

        /// <summary>
        /// 执行不带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdtext">增删改SQL语句或存储过程</param>
        /// <param name="ct">命令类型（SQL语句或存储过程）</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdtext, CommandType ct)
        {//该方法执行传入的SQL语句
            int res = 0;
            try
            {
                using(cmd = new SqlCommand(cmdtext, getconn()))
                {
                    cmd.CommandType = ct;
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return res;
        }

        /// <summary>
        ///  执行带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdtext">增删改SQL语句或存储过程</param>
        /// <param name="paras">参数集合</param>
        /// <param name="ct">命令类型（SQL语句或存储过程）</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdtext, SqlParameter[] paras, CommandType ct)
        {//该方法执行传入的SQL语句
            int res = 0;
            try
            {
                using(cmd = new SqlCommand(cmdtext, getconn()))
                {
                    cmd.CommandType = ct;
                    cmd.Parameters.AddRange(paras);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return res;
        }

        /// <summary>
        /// 执行带参数查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdtext">查询SQL语句或存储过程</param>
        /// <param name="paras">参数集合</param>
        /// <param name="ct">命令类型（SQL语句或存储过程）</param>
        /// <returns></returns>
        public DataTable ExcuteQuery(string cmdtext, SqlParameter[] paras, CommandType ct)
        {//执行查询
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(cmdtext, getconn());
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// 执行不带参数查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdtext">查询SQL语句或存储过程</param>
        /// <param name="ct">命令类型（SQL语句或存储过程）</param>
        /// <returns></returns>
        public DataTable ExcuteQuery(string cmdtext, CommandType ct)
        {
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(cmdtext, getconn());
                cmd.CommandType = ct;
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取结果集中的第一行的第一列，常用于计数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public object ExecuteScalare(string sql, CommandType type, params SqlParameter[] pars)
        {
            using(cmd = new SqlCommand(sql, getconn()))
            {
                cmd.CommandType = type;
                if(pars != null)
                {
                    cmd.Parameters.AddRange(pars);
                }
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
