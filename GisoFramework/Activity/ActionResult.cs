﻿// -----------------------------------------------------------------------
// <copyright file="ActionResult.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Activity
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;

    /// <summary>
    /// Implements a class that represents the result of an operation
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the ActionResult class.
        /// </summary>
        public ActionResult()
        {
            this.Success = false;
            this.MessageError = "No action";
        }

        /// <summary>
        /// Gets a default action result for no action succeded
        /// </summary>
        public static ActionResult NoAction
        {
            get
            {
                return new ActionResult() { Success = false, MessageError = "No action" };
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if the action has is success or fail
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message of result
        /// </summary>
        public string MessageError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the return value of action
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Sets the success of action to true
        /// </summary>
        public void SetSuccess()
        {
            this.SetSuccess(string.Empty);
        }

        /// <summary>
        /// Sets the success of action to true with a message
        /// </summary>
        /// <param name="message">Text of message</param>
        public void SetSuccess(string message)
        {
            this.Success = true;
            this.MessageError = message;
        }

        /// <summary>
        /// Sets the success of action to true with a message
        /// </summary>
        /// <param name="newItemId">Identifier of new item added in database</param>
        public void SetSuccess(int newItemId)
        {
            this.Success = true;
            this.MessageError = newItemId.ToString(CultureInfo.GetCultureInfo("en-us"));
        }

        /// <summary>
        /// Sets the success of action to true with a message
        /// </summary>
        /// <param name="value">Velue of return object</param>
        public void SetSuccess(object value)
        {
            this.Success = true;
            this.MessageError = string.Empty;
            this.ReturnValue = value;
        }

        /// <summary>
        /// Sets the success of action to true with a message
        /// </summary>
        /// <param name="newItemId">Identifier of new item added in database</param>
        public void SetSuccess(long newItemId)
        {
            this.Success = true;
            this.MessageError = newItemId.ToString(CultureInfo.GetCultureInfo("en-us"));
        }

        /// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="message">Text of message</param>
        public void SetFail(string message)
        {
            this.Success = false;
            this.MessageError = message;
        }

        /// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(Exception ex)
        {
            this.Success = false;

            if (ex != null)
            {
                this.MessageError = ex.Message;
            }
            else
            {
                this.MessageError = string.Empty;
            }
        }

        /*/// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(SqlException ex)
        {
            this.Success = false;
            this.MessageError = ex.Message;
        }

        /// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(NullReferenceException ex)
        {
            this.Success = false;
            this.MessageError = ex.Message;
        }

        /// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(FormatException ex)
        {
            this.Success = false;
            this.MessageError = ex.Message;
        }

        /// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(NotSupportedException ex)
        {
            this.Success = false;
            this.MessageError = ex.Message;
        }

        /// <summary>
        /// Sets the success of action to false with a message
        /// </summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(NotImplementedException ex)
        {
            this.Success = false;
            this.MessageError = ex.Message;
        }*/
    }
}
