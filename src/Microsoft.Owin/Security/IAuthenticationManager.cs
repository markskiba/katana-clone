﻿// <copyright file="IAuthenticationManager.cs" company="Microsoft Open Technologies, Inc.">
// Copyright 2011-2013 Microsoft Open Technologies, Inc. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

#if NET45
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.Owin.Security
{
    /// <summary>
    /// Used to interact with authentication middleware that have been chained in the pipeline
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Returns the current user for the request
        /// </summary>
        ClaimsPrincipal User { get; }

        /// <summary>
        /// Lists all of the description data provided by authentication middleware that have been chained
        /// </summary>
        /// <returns></returns>
        IEnumerable<AuthenticationDescription> AuthenticationTypes();

        /// <summary>
        /// Call back through the middleware to ask for a specific form of authentication to be performed
        /// on the current request
        /// </summary>
        /// <param name="authenticationType">Identifies which middleware should respond to the request
        /// for authentication. This value is compared to the middleware's Options.AuthenticationType property.</param>
        /// <returns>Returns an object with the results of the authentication. The AuthenticationResult.Identity
        /// may be null if authentication failed. Even if the Identity property is null, there may still be 
        /// AuthenticationResult.Extra and AuthenticationResult.Description information returned.</returns>
        Task<AuthenticateResult> AuthenticateAsync(string authenticationType);

        /// <summary>
        /// Called to perform any number of authentication mechanisms on the current request.
        /// </summary>
        /// <param name="authenticationTypes">Identifies one or more middleware which should attempt to respond</param>
        /// <returns>Returns the AuthenticationResult information from the middleware which responded. The 
        /// order is determined by the order the middleware are in the pipeline. Latest added is first in the list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Returning an IEnumerable in a Task")]
        Task<IEnumerable<AuthenticateResult>> AuthenticateAsync(string[] authenticationTypes);

        /// <summary>
        /// Add information into the response environment that will cause the authentication middleware to challenge
        /// the caller to authenticate. This also changes the status code of the response to 401. The nature of that 
        /// challenge varies greatly, and ranges from adding a response header or changing the 401 status code to 
        /// a 302 redirect.
        /// </summary>
        /// <param name="authenticationTypes">Identify which middleware should perform their alterations on the
        /// response. If the authenticationTypes is null or empty, that means the 
        /// AuthenticationMode.Active middleware should perform their alterations on the response.</param>
        /// <param name="extra">Additional arbitraty values which may be used by particular authentication types.</param>
        void Challenge(string[] authenticationTypes, AuthenticationExtra extra);

        /// <summary>
        /// Add information to the response environment that will cause the appropriate authentication middleware
        /// to grant a claims-based identity to the recipient of the response. The exact mechanism of this may vary.
        /// Examples include setting a cookie, to adding a fragment on the redirect url, or producing an OAuth2
        /// access code or token response.
        /// </summary>
        /// <param name="user">Determines which claims are granted to the signed in user. The 
        /// ClaimsIdentity.AuthenticationType property is compared to the middleware's Options.AuthenticationType 
        /// value to determins which claims are granted by which middleware. The recommended use is to have a single
        /// ClaimsIdentity which has the AuthenticationType matching a specific middleware.</param>
        /// <param name="extra">Contains additional properties the middleware are expected to persist along with
        /// the claims. These values will be returned as the AuthenticateResult.Extra collection when AuthenticateAsync
        /// is called on subsequent requests.</param>
        void SignIn(ClaimsPrincipal user, AuthenticationExtra extra);

        /// <summary>
        /// Add information to the response environment that will cause the appropriate authentication middleware
        /// to revoke any claims identity associated the the caller. The exact method varies.
        /// </summary>
        /// <param name="authenticationTypes">Identifies which middleware should perform the work to sign out.
        /// Multiple authentication types may be provided to clear out more than one cookie at a time, or to clear
        /// cookies and redirect to an external single-sign out url.</param>
        void SignOut(params string[] authenticationTypes);
    }
}
#endif