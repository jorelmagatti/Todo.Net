using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Dtos;
using Todo.Services.Interfaces;

namespace Todo.Services.Services
{
    public abstract class Service : IServiceScoped
    {
        #region Parameters
        protected readonly ILogger _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Messages { get; set; } = new List<string>();
        #endregion

        #region Constructor
        public Service(ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
            Errors = new List<string>();
            Messages = new List<string>();
        }
        #endregion

        #region Protected Methods
        protected void AddLogToAmbient(Exception ex)
        {
            if (ex.InnerException is not null)
            {
                _logger.LogError(ex.InnerException.Message);
                _logger.LogError(ex.InnerException.StackTrace == null ? "" : ex.InnerException.StackTrace);
            }

            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace == null ? "" : ex.StackTrace);
        }

        protected void AddLogToAmbient(Exception ex, string fullMethodName)
        {
            if (ex.InnerException is not null)
            {
                _logger.LogError(ex.InnerException.Message + "| Full Method: " + fullMethodName);
                _logger.LogError(ex.InnerException.StackTrace == null ? "" : ex.InnerException.StackTrace);
            }

            _logger.LogError(ex.Message + "| Full Method: " + fullMethodName);
            _logger.LogError(ex.StackTrace == null ? "" : ex.StackTrace);
        }

        protected void AddLogToBase(Exception ex)
        {
            if (ex.InnerException is not null)
            {
                Errors.Add(ex.InnerException.Message);
                Errors.Add(ex.InnerException.StackTrace == null ? "" : ex.InnerException.StackTrace);
            }

            Errors.Add(ex.Message);
            Errors.Add(ex.StackTrace == null ? "" : ex.StackTrace);
        }

        protected void AddLogToBase(Exception ex, string fullMethodName)
        {
            if (ex.InnerException is not null)
            {
                Errors.Add(ex.InnerException.Message + "| Full Method: " + fullMethodName);
                Errors.Add(ex.InnerException.StackTrace == null ? "" : ex.InnerException.StackTrace);
            }

            Errors.Add(ex.Message + "| Full Method: " + fullMethodName);
            Errors.Add(ex.StackTrace == null ? "" : ex.StackTrace);
        }

        protected DateTime HrPtBrByPlataform()
        {
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo hrBrasilia;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            else
                hrBrasilia = TimeZoneInfo.Local;

            DateTime retorno = TimeZoneInfo.ConvertTimeFromUtc(dateTime, hrBrasilia);
            return retorno;
        }

        public UserDto? GetSessionUser()
        {
            try
            {
                byte[] bytes = [];
                _httpContextAccessor.HttpContext.Session.TryGetValue("user", out bytes);
                string value = System.Text.Encoding.Default.GetString(bytes);
                return value is not null ? JsonConvert.DeserializeObject<UserDto>(value) : null;
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }
        #endregion
    }
}
