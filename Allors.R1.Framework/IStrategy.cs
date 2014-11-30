//------------------------------------------------------------------------------------------------- 
// <copyright file="IStrategy.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the IStrategy type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1
{
    using Allors.R1.Meta;

    /// <summary>
    /// A strategy based object delegates all framework related work
    /// to its strategy object.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        ISession Session { get; }

        /// <summary>
        /// Gets the database session.
        /// </summary>
        /// <value>The database session.</value>
        IDatabaseSession DatabaseSession { get; }

        /// <summary>
        /// Gets the <see cref="ObjectType"/>.
        /// </summary>
        /// <value>The object type.</value>
        ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the <see cref="R1.ObjectId"/>.
        /// </summary>
        /// <value>The object id.</value>
        ObjectId ObjectId { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        bool IsDeleted { get; }

        /// <summary>
        /// Gets a value indicating whether this object is new in the current Session.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this object is new in the current session otherwise, <c>false</c>.
        /// </value>
        bool IsNewInSession { get; }

        /// <summary>
        /// Gets a value indicating whether this object is new in the workspace.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this object is new in the workspace otherwise, <c>false</c>.
        /// </value>
        bool IsNewInWorkspace { get; }

        /// <summary>
        /// Gets the <see cref="IObject"/>.
        /// </summary>
        /// <returns>The allors object.</returns>
        IObject GetObject();
        
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        void Delete();

        /// <summary>
        /// Gets a value indicating whether the composite role exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c> </returns>
        bool ExistRole(RoleType roleType);

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        object GetRole(RoleType roleType);

        /// <summary>
        /// Sets the role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="value">The value.</param>
        void SetRole(RoleType roleType, object value);

        /// <summary>
        /// Removes the role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveRole(RoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the unit role exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the unit role exists; otherwise,<c>false</c> </returns>
        bool ExistUnitRole(RoleType roleType);

        /// <summary>
        /// Gets the unit role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        object GetUnitRole(RoleType roleType);

        /// <summary>
        /// Sets the unit role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="unit">The unit .</param>
        void SetUnitRole(RoleType roleType, object unit);

        /// <summary>
        /// Removes the unit role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveUnitRole(RoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the composite role exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c> </returns>
        bool ExistCompositeRole(RoleType roleType);

        /// <summary>
        /// Gets the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        IObject GetCompositeRole(RoleType roleType);

        /// <summary>
        /// Sets the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="composite">The composite.</param>
        void SetCompositeRole(RoleType roleType, IObject composite);

        /// <summary>
        /// Removes the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveCompositeRole(RoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the composite roles exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c> </returns>
        bool ExistCompositeRoles(RoleType roleType);

        /// <summary>
        /// Gets the composite roles.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role objects.</returns>
        Extent GetCompositeRoles(RoleType roleType);

        /// <summary>
        /// Adds the composite role.
        /// </summary>
        /// <param name="roleType">The relation type..</param>
        /// <param name="objectToAdd">The object to add.</param>
        void AddCompositeRole(RoleType roleType, IObject objectToAdd);

        /// <summary>
        /// Removes the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="objectToRemove">The object to remove.</param>
        void RemoveCompositeRole(RoleType roleType, IObject objectToRemove);

        /// <summary>
        /// Sets the composite roles.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="roles">The roles.</param>
        void SetCompositeRoles(RoleType roleType, Extent roles);

        /// <summary>
        /// Removes the composite roles.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveCompositeRoles(RoleType roleType);
        
        /// <summary>
        /// Gets a value indicating whether the association exists.
        /// </summary>
        /// <param name="associationType">The relation type.</param>
        /// <returns><c>true</c>if the association exists; otherwise,<c>false</c> </returns>
        bool ExistAssociation(AssociationType associationType);

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The association object.</returns>
        object GetAssociation(AssociationType roleType);

        /// <summary>
        /// Gets a value indicating whether the composite association exists.
        /// </summary>
        /// <param name="associationType">The relation type.</param>
        /// <returns><c>true</c>if the composite association exists; otherwise,<c>false</c> </returns>
        bool ExistCompositeAssociation(AssociationType associationType);

        /// <summary>
        /// Gets the composite association.
        /// </summary>
        /// <param name="associationType">Type of the relation.</param>
        /// <returns>The association object.</returns>
        IObject GetCompositeAssociation(AssociationType associationType);

        /// <summary>
        /// Gets a value indicating whether the composite associations exists.
        /// </summary>
        /// <param name="associationType">The relation type.</param>
        /// <returns><c>true</c>if the composite associations exists; otherwise,<c>false</c> </returns>
        bool ExistCompositeAssociations(AssociationType associationType);

        /// <summary>
        /// Gets the composite associations.
        /// </summary>
        /// <param name="associationType">Type of the relation.</param>
        /// <returns>The association objects.</returns>
        Extent GetCompositeAssociations(AssociationType associationType);
    }
}