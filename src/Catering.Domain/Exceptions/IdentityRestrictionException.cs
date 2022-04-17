using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions
{
    [Serializable]
    public class IdentityRestrictionException : CateringException
    {
        public IdentityRestrictionException(string identityId, string actionName) 
            : base($"Identity ({identityId}) is not allowed to perform action ({actionName}) based on persmissions.") { }

        protected IdentityRestrictionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
