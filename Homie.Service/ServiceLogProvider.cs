#region License

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceLogProvider.cs" company="https://github.com/lemked/homieremotedesktop">
// Copyright (C) 2014 Daniel Lemke
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// </copyright>
// <author>Daniel Lemke</author>
// <email>lemked@web.de</email>
// <summary>TODO: Add summary</summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Homie.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceProcess;
    using System.Threading.Tasks;

    using Homie.Common;
    using Homie.Common.Interfaces;
    using Homie.Model.Logging;

    /// <summary>
    /// The service log provider.
    /// </summary>
    public class ServiceLogProvider : IServiceLogProvider
    {
        #region Constants

        /// <summary>
        /// The service name.
        /// </summary>
        private const string ServiceName = Constants.ServiceName;

        #endregion

        #region Fields

        /// <summary>
        /// The data source.
        /// </summary>
        private readonly IServiceLogDataSource dataSource;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogProvider"/> class.
        /// </summary>
        public ServiceLogProvider()
            : this(DependencyInjector.Resolve<IServiceLogDataSource>())
        {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogProvider"/> class.
        /// </summary>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        public ServiceLogProvider(IServiceLogDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        #endregion

        #region Public Methods and Operators


        /// <summary>
        /// Authenticates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if authenticated successfully, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Authenticate(string user, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The connect async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The get log entries async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IEnumerable<LogMessage>> GetLogEntriesAsync()
        {
            return await Task.Factory.StartNew(() => this.GetLogEntries());
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get log entries.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<LogMessage> GetLogEntries()
        {
            return this.dataSource.LogMessages;
        }

        #endregion
    }
}