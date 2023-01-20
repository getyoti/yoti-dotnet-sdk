﻿using System;
using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;
using Yoti.Auth.DocScan.Session.Create.Task;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class SessionSpecificationBuilder
    {
        private readonly List<BaseRequestedCheck> _requestedChecks = new List<BaseRequestedCheck>();
        private readonly List<BaseRequestedTask> _requestedTasks = new List<BaseRequestedTask>();
        private int? _clientSessionTokenTtl;
        private int? _resourcesTtl;
        private string _userTrackingId;
        private NotificationConfig _notifications;
        private SdkConfig _sdkConfig;
        private List<RequiredDocument> _requiredDocuments;
        private bool? _blockBiometricConsent;
        private DateTimeOffset? _sessionDeadline;
        private object _identityProfileRequirements;
        private object _subject;
        private bool _createIdentityProfilePreview;

        /// <summary>
        /// Sets the client session token TTL (time-to-live)
        /// </summary>
        /// <remarks>
        /// Can be used as an alternative to <see cref="WithSessionDeadline"/> 
        /// </remarks>
        /// <param name="clientSessionTokenTtl">the client session token TTL</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithClientSessionTokenTtl(int clientSessionTokenTtl)
        {
            _clientSessionTokenTtl = clientSessionTokenTtl;
            return this;
        }

        /// <summary>
        /// Sets the resources TTL (time-to-live)
        /// </summary>
        /// <param name="resourcesTtl">the resources TTL</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithResourcesTtl(int resourcesTtl)
        {
            _resourcesTtl = resourcesTtl;
            return this;
        }

        /// <summary>
        /// Sets the user tracking ID
        /// </summary>
        /// <param name="userTrackingId">the user tracking ID</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithUserTrackingId(string userTrackingId)
        {
            _userTrackingId = userTrackingId;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="NotificationConfig"/>
        /// </summary>
        /// <param name="notifications">The notification configuration</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithNotifications(NotificationConfig notifications)
        {
            _notifications = notifications;
            return this;
        }

        /// <summary>
        /// Adds a <see cref="BaseRequestedCheck"/> to the list
        /// </summary>
        /// <param name="requestedCheck">The requested check</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithRequestedCheck(BaseRequestedCheck requestedCheck)
        {
            _requestedChecks.Add(requestedCheck);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="BaseRequestedCheck"/> to the list
        /// </summary>
        /// <param name="requestedTask">The requested task</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithRequestedTask(BaseRequestedTask requestedTask)
        {
            _requestedTasks.Add(requestedTask);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="SdkConfig"/>
        /// </summary>
        /// <param name="sdkConfig">The <see cref="SdkConfig"/></param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithSdkConfig(SdkConfig sdkConfig)
        {
            _sdkConfig = sdkConfig;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="RequiredDocument"/>
        /// </summary>
        /// <param name="requiredDocument">The <see cref="RequiredDocument"/></param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithRequiredDocument(RequiredDocument requiredDocument)
        {
            if (_requiredDocuments == null)
                _requiredDocuments = new List<RequiredDocument>();

            _requiredDocuments.Add(requiredDocument);
            return this;
        }

        /// <summary>
        /// Sets whether or not to block the collection of biometric consent
        /// </summary>
        /// <param name="blockBiometricConsent">Boolean value to choose whether to block biometric consent</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithBlockBiometricConsent(bool blockBiometricConsent)
        {
            _blockBiometricConsent = blockBiometricConsent;
            return this;
        }

        /// <summary>
        /// Sets the deadline that the session needs to be completed by
        /// </summary>
        /// <remarks>
        /// Can be used as an alternative to <see cref="WithClientSessionTokenTtl"/> 
        /// </remarks>
        /// <param name="sessionDeadline">The <see cref="DateTimeOffset"/> for the session to end</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithSessionDeadline(DateTimeOffset sessionDeadline)
        {
            _sessionDeadline = sessionDeadline;
            return this;
        }

        /// <summary>
        /// Sets the Identity Profile Requirements for the session
        /// </summary>
        /// <param name="identityProfileRequirements">The Identity Profile Requirements <see cref="object"/> for the session</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithIdentityProfileRequirements(object identityProfileRequirements)
        {
            _identityProfileRequirements = identityProfileRequirements;
            return this;
        }

        /// <summary>
        /// Sets the Subject object for the session
        /// </summary>
        /// <param name="subject">The Subject <see cref="object"/> for the session</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithSubject(object subject)
        {
            _subject = subject;
            return this;
        }

        /// <summary>
        /// Sets the Identity Profile Requirements for the session
        /// </summary>
        /// <param name="identityProfileRequirements">The Identity Profile Requirements <see cref="object"/> for the session</param>
        /// <returns>the builder</returns>
        public SessionSpecificationBuilder WithCreateIdentityProfilePreview(bool createIdentityProfilePreview)
        {
            _createIdentityProfilePreview = createIdentityProfilePreview;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="SessionSpecification"/> based on the values supplied to the builder
        /// </summary>
        /// <returns>The built <see cref="SessionSpecification"/></returns>
        public SessionSpecification Build()
        {
            return new SessionSpecification(
                _clientSessionTokenTtl,
                _resourcesTtl,
                _userTrackingId,
                _notifications,
                _requestedChecks,
                _requestedTasks,
                _sdkConfig,
                _requiredDocuments,
                _blockBiometricConsent,
                _sessionDeadline,
                _identityProfileRequirements,
                _subject,
                _createIdentityProfilePreview
                );
        }
    }
}
