using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ActionNotAllowedException : CateringException
{
    public ActionNotAllowedException() : base() { }

    public ActionNotAllowedException(string nameOfAction) : base($"Action {nameOfAction} is not allowed based on domain rules.") { }

    protected ActionNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
