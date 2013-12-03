// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service Base.
    /// </summary>
    public class BaseService
    {
        protected readonly IDynamicRepository DynamicRepository;

        public BaseService(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        public IServiceContext ServiceContext { get; set; }

        public CommandResult InternalError
        {
            get
            {
                return new CommandResult
                {
                    Code = 500,
                    Message = Messages.InternalError
                };
            }
        }

        /// <summary>
        ///  Gets dynamic repository.
        /// </summary>
        /// <value>Dynamic Repository contract.</value>
        protected IDynamicRepository Repository
        {
            get { return DynamicRepository; }
        }

        /// <summary>
        /// Gets report data that are satisfied the passed criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            var result = new List<T>();

            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                result = DynamicRepository.Fetch<T>(criteria).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets item that are satisfied the passed criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual T GetItem<T>(ICriteria criteria) where T : new()
        {
            var result = new T();
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                result = DynamicRepository.Get<T>(criteria);
            }

            return result;
        }
    }
}
