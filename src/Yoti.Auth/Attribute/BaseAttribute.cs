using System.Collections.Generic;
using System.Linq;
using Yoti.Auth.Anchors;

namespace Yoti.Auth.Attribute
{
    /// <summary>
    /// A class to represent a base Yoti attribute, without any generics. A Yoti attribute consists
    /// of the attribute name, an associated <see cref="YotiAttributeValue"/>, and a list of <see
    /// cref="Anchor"/> from Yoti. It may hold one or more anchors, which specify how an attribute
    /// has been provided and how it has been verified within the Yoti platform.
    /// </summary>
    public abstract class BaseAttribute
    {
        private readonly string _name;
        private readonly List<Anchor> _anchors;

        private protected BaseAttribute(string name, List<Anchor> anchors)
        {
            _name = name;
            _anchors = anchors;
        }

        /// <summary>
        /// Gets the name of the attribute
        /// </summary>
        /// <returns>Attribute name</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Get the anchors for an attribute. If an attribute has only one SOURCE Anchor with the
        /// value set to "USER_PROVIDED" and zero VERIFIER Anchors, then the attribute is a
        /// self-certified one.
        /// </summary>
        /// <returns>A list of all of the anchors associated with an attribute</returns>
        public List<Anchor> GetAnchors()
        {
            return _anchors.ToList();
        }

        /// <summary>
        /// Sources are a subset of the anchors associated with an attribute, where the anchor type
        /// is <see cref="AnchorType.SOURCE"/>".
        /// </summary>
        /// <returns>A list of <see cref="AnchorType.SOURCE"/>" anchors</returns>
        public List<Anchor> GetSources()
        {
            return _anchors.Where(a => a.GetAnchorType() == AnchorType.SOURCE).ToList();
        }

        /// <summary>
        /// Verifiers are a subset of the anchors associated with an attribute, where the anchor type
        /// is <see cref="AnchorType.VERIFIER"/>".
        /// </summary>
        /// <returns>A list of <see cref="AnchorType.VERIFIER"/>" anchors</returns>
        public List<Anchor> GetVerifiers()
        {
            return _anchors.Where(a => a.GetAnchorType() == AnchorType.VERIFIER).ToList();
        }
    }
}