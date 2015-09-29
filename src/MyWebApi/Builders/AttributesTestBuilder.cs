﻿// MyWebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.

namespace MyWebApi.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base;
    using Common.Extensions;
    using Contracts.Attributes;
    using Exceptions;
    using Utilities;

    public class AttributesTestBuilder : BaseTestBuilder, IAndAttributesTestBuilder
    {
        private readonly ICollection<Action<IEnumerable<object>>> validations;

        public AttributesTestBuilder(ApiController controller, string actionName)
            : base(controller, actionName)
        {
            this.validations = new List<Action<IEnumerable<object>>>();
        }

        public IAndAttributesTestBuilder ContainingAttributeOfType<TAttribute>()
            where TAttribute : Attribute
        {
            var expectedAttributeType = typeof(TAttribute);
            this.validations.Add(attr =>
            {
                if (attr.All(a => a.GetType() != expectedAttributeType))
                {
                    throw new AttributeAssertionException(string.Format(
                        "When calling {0} action in {1} expected action to have {2} attribute, but in fact none was found.",
                        this.Controller.GetName(),
                        this.ActionName,
                        expectedAttributeType.ToFriendlyTypeName()));
                }
            });
            return this;
        }

        public IAttributesTestBuilder AndAlso()
        {
            return this;
        }

        internal ICollection<Action<IEnumerable<object>>> GetAttributeValidations()
        {
            return this.validations;
        }
    }
}
