﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestOfConventionTypeRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string conventionTypeCode)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            if (request == null)
            {
                return false;
            }

            return request.ConventionType?.Code == conventionTypeCode;
        }
    }
}
