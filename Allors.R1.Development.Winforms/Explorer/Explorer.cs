// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Explorer.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml;

    using Allors.R1.Development.Repository;
    using Allors.R1.Development.Repository.Generation;
    using Allors.R1.Development.Repository.Meta;
    using Allors.R1.Development.Repository.WinForms.Decorators;
    using Allors.R1.Development.Repository.WinForms.Dialogs;
    using Allors.R1.Development.Repository.WinForms.Wizards;
    using Allors.R1.Meta;

    public class Explorer : UserControl
    {
        private const int ImageAllors = 0;
        private const int ImageTemplates = 1;
        private const int ImageDomain = 2;
        private const int ImageClass = 3;
        private const int ImageInterface = 4;
        private const int ImageAssociation = 5;
        private const int ImageRole = 6;
        private const int ImageMethod = 7;
        private const int ImageMethodLocked = 8;
        private const int ImageFilter = 9;
        private const int ImageNamespace = 10;
        private const int ImageFolderClosed = 11;
        private const int ImageFolderOpened = 12;
        private const int ImageNamespaceLocked = 13;
        private const int ImageClassLocked = 14;
        private const int ImageInterfaceLocked = 15;
        private const int ImageRoleLocked = 16;
        
        private const string TitleRepositories = "Repositories";
        private const string TitleSuperDomains = "Super Domains";
        private const string TitleSupertypes = "Supertypes";
        private const string TitleTemplates = "Templates";

        private readonly TreeNode repositoriesNode;
        private readonly List<Repository> repositories;

        private IContainer components;
        private ImageList imageList;

        private ContextMenu contextMenu;

        private bool newMenuEnabled;
        private bool openMenuEnabled;

        private MenuItem generateMenuItem;
        private MenuItem openRepositoryMenuItem;
        private MenuItem addRepositoryMenuItem;
        private MenuItem addTemplateMenuItem;
        private MenuItem addSuperDomainMenuItem;
        private MenuItem addNamespaceMenuItem;
        private MenuItem addTypeMenuItem;
        private MenuItem addRelationMenuItem;
        private MenuItem updateTemplateMenuItem;
        private MenuItem deleteMenuItem;
        private MenuItem extraMenuItem;
        private MenuItem cleanUpMenuItem;

        private TreeNode nodeSelection;
        private RepositoriesTag repositoriesSelection;
        private RepositoryTag repositorySelection;
        private SuperDomainsTag superDomainsSelection;
        private SuperDomainTag superDomainSelection;
        private TemplatesTag templatesSelection;
        private TemplateTag templateSelection;
        private DomainTag domainSelection;
        private ObjectTypeTag objectTypeSelection;
        private InheritancesTag inheritancesSelection;
        private InheritanceTag inheritanceSelection;
        private RelationTypeTag relationTypeSelection;

        private TreeView treeView;
        private ToolBar toolBar;
        private MenuItem pullUpMenuItem;
        private MenuItem pushDownMenuItem;
        private MenuItem removeMenuItem;
        private ToolBarButton toolBarButton;

        public Explorer()
        {
            this.InitializeComponent();

            this.treeView.ShowRootLines = false;
            this.treeView.TreeViewNodeSorter = new ExplorerSorter();

            this.repositories = new List<Repository>();

            this.repositoriesNode = this.treeView.Nodes.Add(TitleRepositories);
            this.repositoriesNode.Tag = new RepositoriesTag();
            this.repositoriesNode.ImageIndex = ImageAllors;
            this.repositoriesNode.SelectedImageIndex = ImageAllors;

            this.treeView.ExpandAll();

            this.treeView.SelectedNode = null;
        }

        [Category("Allors")]
        public event EventHandler<SelectedEventArgs> Selected;

        [Category("Allors")]
        public event EventHandler<RepositoryObjectChangedEventArgs> RepositoryObjectChanged;

        [Category("Allors")]
        public event EventHandler<RepositoryObjectDeletedEventArgs> RepositoryObjectDeleted;

        [Category("Allors")]
        public event EventHandler<RepositoryMetaObjectChangedEventArgs> MetaObjectChanged;

        [Category("Allors")]
        public event EventHandler<RepositoryMetaObjectDeletedEventArgs> MetaObjectDeleted;

        public bool ExtendedViewEnabled
        {
            get
            {
                return this.toolBarButton.Pushed;
            }
        }

        public bool NewMenuEnabled
        {
            get
            {
                return this.newMenuEnabled;
            }

            set
            {
                this.newMenuEnabled = value;
                this.SyncRepositories();
            }
        }

        public bool OpenMenuEnabled
        {
            get
            {
                return this.openMenuEnabled;
            }

            set
            {
                this.openMenuEnabled = value;
                this.SyncRepositories();
            }
        }

        public Repository[] Repositories
        {
            get { return this.repositories.ToArray(); }
        }

        public void AddRepository(Repository repository)
        {
            this.repositories.Add(repository);

            repository.MetaObjectChanged += this.OnMetaObjectChanged;
            repository.MetaObjectDeleted += this.OnMetaObjectDeleted;

            repository.ObjectChanged += this.OnObjectChanged;
            repository.ObjectDeleted += this.OnObjectDeleted;

            this.SyncRepositories();
        }

        public void RemoveRepository(Repository repository)
        {
            repository.MetaObjectChanged -= this.OnMetaObjectChanged;
            repository.MetaObjectDeleted -= this.OnMetaObjectDeleted;

            repository.ObjectChanged -= this.OnObjectChanged;
            repository.ObjectDeleted -= this.OnObjectDeleted;

            this.repositories.Remove(repository);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void ProcessSelection(TreeNode node)
        {
            this.nodeSelection = node;

            foreach (MenuItem menuItem in this.contextMenu.MenuItems)
            {
                menuItem.Visible = false;
            }

            this.repositoriesSelection = null;
            this.repositorySelection = null;
            this.superDomainsSelection = null;
            this.superDomainSelection = null;
            this.templatesSelection = null;
            this.templateSelection = null;
            this.domainSelection = null;
            this.objectTypeSelection = null;
            this.inheritancesSelection = null;
            this.inheritanceSelection = null;
            this.relationTypeSelection = null;

            if (node == null)
            {
                object selectionForPropertyGrid = new EmptyDecorator();
                if (this.Selected != null)
                {
                    this.Selected(this, new SelectedEventArgs(null, selectionForPropertyGrid));
                }
            }
            else
            {
                var tag = node.Tag;

                this.repositoriesSelection = tag as RepositoriesTag;
                this.repositorySelection = tag as RepositoryTag;

                this.templatesSelection = tag as TemplatesTag;
                this.templateSelection = tag as TemplateTag;
                this.superDomainsSelection = tag as SuperDomainsTag;
                this.superDomainSelection = tag as SuperDomainTag;

                this.objectTypeSelection = tag as ObjectTypeTag;
                this.inheritancesSelection = tag as InheritancesTag;
                this.inheritanceSelection = tag as InheritanceTag;
                this.domainSelection = tag as DomainTag;
                this.relationTypeSelection = tag as RelationTypeTag;

                if (this.repositoriesSelection != null)
                {
                    this.addRepositoryMenuItem.Visible = true;
                    this.openRepositoryMenuItem.Visible = true;
                }

                if (this.repositorySelection != null)
                {
                    this.generateMenuItem.Visible = true;
                    this.addNamespaceMenuItem.Visible = true;
                    this.extraMenuItem.Visible = true;
                    this.cleanUpMenuItem.Visible = true;
                }

                if (this.superDomainsSelection != null)
                {
                    this.addSuperDomainMenuItem.Visible = true;
                }

                if (this.superDomainSelection != null)
                {
                    this.deleteMenuItem.Visible = true;
                }

                if (this.templatesSelection != null)
                {
                    this.addTemplateMenuItem.Visible = true;
                }

                if (this.templateSelection != null)
                {
                    this.updateTemplateMenuItem.Visible = true;
                    this.generateMenuItem.Visible = true;
                    this.deleteMenuItem.Visible = true;
                }

                if (this.domainSelection != null)
                {
                    this.addTypeMenuItem.Visible = true;
                    if (this.domainSelection.Domain.IsSuperDomain)
                    {
                        this.deleteMenuItem.Visible = true;
                    }
                }

                if (this.objectTypeSelection != null)
                {
                    this.addRelationMenuItem.Visible = true;
                    if (!this.objectTypeSelection.ObjectType.DomainWhereDeclaredObjectType.IsSuperDomain)
                    {
                        this.deleteMenuItem.Visible = true;
                        this.pullUpMenuItem.Visible = true;
                    }
                    else
                    {
                        this.pushDownMenuItem.Visible = true;
                    }
                }

                if (this.inheritancesSelection != null)
                {
                }

                if (this.inheritanceSelection != null)
                {
                    if (!this.inheritanceSelection.Inheritance.DomainWhereDeclaredInheritance.IsSuperDomain)
                    {
                        this.removeMenuItem.Visible = true;
                        this.pullUpMenuItem.Visible = true;
                    }
                    else
                    {
                        this.pushDownMenuItem.Visible = true;
                    }
                }

                if (this.relationTypeSelection != null)
                {
                    if (!this.relationTypeSelection.RelationType.DomainWhereDeclaredRelationType.IsSuperDomain)
                    {
                        this.deleteMenuItem.Visible = true;
                        this.pullUpMenuItem.Visible = true;
                    }
                    else
                    {
                        this.pushDownMenuItem.Visible = true;
                    }
                }

                object selectionForPropertyGrid = new EmptyDecorator();
                if (this.repositorySelection != null)
                {
                    selectionForPropertyGrid = new RepositoryDecorator(this.repositorySelection.Repository);
                }
                else if (this.superDomainSelection != null)
                {
                    selectionForPropertyGrid = new SuperDomainDecorator(this.superDomainSelection.SuperDomain);
                }
                else if (this.templateSelection != null)
                {
                    selectionForPropertyGrid = new TemplateDecorator(this.templateSelection.Template);
                }
                else if (this.domainSelection != null)
                {
                    selectionForPropertyGrid = new DomainDecorator(this.domainSelection.Domain);
                }
                else if (this.objectTypeSelection != null)
                {
                    if (!this.objectTypeSelection.ObjectType.DomainWhereDeclaredObjectType.IsSuperDomain)
                    {
                        selectionForPropertyGrid = new ObjectTypeDecorator(this.objectTypeSelection.ObjectType);
                    }
                    else
                    {
                        if (this.objectTypeSelection.ObjectType.DirectSuperclass != null)
                        {
                            var inheritance = this.objectTypeSelection.ObjectType.FindInheritanceWhereDirectSubtype(this.objectTypeSelection.ObjectType.DirectSuperclass);
                            if (!inheritance.DomainWhereDeclaredInheritance.IsSuperDomain)
                            {
                                selectionForPropertyGrid = new ObjectTypeSuperClassLockedDecorator(this.objectTypeSelection.ObjectType);
                            }
                            else
                            {
                                selectionForPropertyGrid = new ObjectTypeLockedDecorator(this.objectTypeSelection.ObjectType);
                            }
                        }
                        else
                        {
                            selectionForPropertyGrid = new ObjectTypeLockedDecorator(this.objectTypeSelection.ObjectType);
                        }
                    }
                }
                else if (this.relationTypeSelection != null)
                {
                    if (!this.relationTypeSelection.RelationType.DomainWhereDeclaredRelationType.IsSuperDomain)
                    {
                        selectionForPropertyGrid = new RelationTypeDecorator(this.relationTypeSelection.RelationType);
                    }
                    else
                    {
                        selectionForPropertyGrid = new RelationTypeLockedDecorator(this.relationTypeSelection.RelationType);
                    }
                }

                if (this.Selected != null)
                {
                    this.Selected(this, new SelectedEventArgs(tag, selectionForPropertyGrid));
                }
            }
        }

        private void DeleteSelection()
        {
            if (this.superDomainSelection != null)
            {
                if (MessageBox.Show("Do you really want to delete " + this.superDomainSelection.SuperDomain + "?", "Allors Explorer - Delete super domain", MessageBoxButtons.OKCancel, MessageBoxIcon.Question).Equals(DialogResult.OK))
                {
                    var superDomain = this.superDomainSelection.SuperDomain;
                    this.superDomainSelection.Repository.RemoveSuper(superDomain);
                }
            }
            else if (this.domainSelection != null)
            {
                if (MessageBox.Show("Do you really want to delete " + this.domainSelection.Domain + "?", "Allors Explorer - Delete namespace", MessageBoxButtons.OKCancel, MessageBoxIcon.Question).Equals(DialogResult.OK))
                {
                    var @namespace = this.domainSelection.Domain;
                    @namespace.Delete();
                }
            }
            else if (this.objectTypeSelection != null)
            {
                if (MessageBox.Show("Do you really want to delete " + this.objectTypeSelection.ObjectType + " and all its relations?", "Allors Explorer - Delete type", MessageBoxButtons.OKCancel, MessageBoxIcon.Question).Equals(DialogResult.OK))
                {
                    var objectType = this.objectTypeSelection.ObjectType;
                    objectType.DeleteRecursive();
                }
            }
            else if (this.relationTypeSelection != null)
            {
                if (MessageBox.Show("Do you really want to delete " + this.relationTypeSelection.RelationType + "?", "Allors Explorer - Delete relation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question).Equals(DialogResult.OK))
                {
                    var relationType = this.relationTypeSelection.RelationType;
                    relationType.Delete();
                }
            }
            else if (this.templateSelection != null)
            {
                if (MessageBox.Show("Do you really want to delete " + this.templateSelection.Template + "?", "Allors Explorer - Delete template", MessageBoxButtons.OKCancel, MessageBoxIcon.Question).Equals(DialogResult.OK))
                {
                    var template = this.templateSelection.Template;
                    template.Delete();
                }
            }
        }

        #region Sync
        private void SyncRepositories(bool syncRoot = true)
        {
            if (syncRoot)
            {
                this.treeView.BeginUpdate();
            }

            this.repositoriesNode.Retain(this.repositories.ConvertAll(repository => new RepositoryTag(repository)));
            foreach (var repository in this.repositories)
            {
                var repositoryNode = this.repositoriesNode.Find(new RepositoryTag(repository));
                if (repositoryNode == null)
                {
                    repositoryNode = new TreeNode
                    {
                        Text = repository.Domain.Name,
                        Tag = new RepositoryTag(repository),
                        ImageIndex = ImageDomain,
                        SelectedImageIndex = ImageDomain
                    };

                    this.repositoriesNode.Nodes.Add(repositoryNode);
                }

                this.SyncRepository(repositoryNode, false);
            }

            this.repositoriesNode.Expand();

            if (syncRoot)
            {
                this.treeView.Sort();
                this.treeView.EndUpdate();
            }
        }

        private void SyncRepository(Repository repository)
        {
            var repositoryNode = this.repositoriesNode.Find(new RepositoryTag(repository));
            if (repositoryNode == null)
            {
                this.SyncRepositories();
            }
            else
            {
                this.SyncRepository(repositoryNode, true);
            }
        }

        private void SyncRepository(TreeNode repositoryNode, bool syncRoot)
        {
            if (syncRoot)
            {
                this.treeView.BeginUpdate();
            }

            var repository = ((RepositoryTag)repositoryNode.Tag).Repository;
            var domain = repository.Domain;

            repositoryNode.SyncText(domain.Name);
            repositoryNode.SyncExpand();

            var superDomainsNode = repositoryNode.Find(new SuperDomainsTag(repository));
            var templatesNode = repositoryNode.Find(new TemplatesTag(repository));

            if (this.ExtendedViewEnabled)
            {
                if (templatesNode == null)
                {
                    templatesNode = new TreeNode
                    {
                        Text = TitleTemplates,
                        Tag = new TemplatesTag(repository),
                        ImageIndex = ImageFolderClosed,
                        SelectedImageIndex = ImageFolderClosed
                    };

                    repositoryNode.Nodes.Insert(0, templatesNode);
                }

                var templates = ((TemplatesTag)templatesNode.Tag).Repository.Templates;
                templatesNode.Retain(Array.ConvertAll(templates, template => new TemplateTag(repository, template)));
                foreach (var template in templates)
                {
                    var templateNode = templatesNode.Find(new TemplateTag(repository, template));
                    if (templateNode == null)
                    {
                        templateNode = new TreeNode
                            {
                                Tag = new TemplateTag(repository, template),
                                ImageIndex = ImageTemplates,
                                SelectedImageIndex = ImageTemplates
                            };

                        templatesNode.Nodes.Add(templateNode);
                    }

                    templateNode.SyncText(template.Name);
                }

                if (superDomainsNode == null)
                {
                    superDomainsNode = new TreeNode
                    {
                        Text = TitleSuperDomains,
                        Tag = new SuperDomainsTag(repository),
                        ImageIndex = ImageFolderClosed,
                        SelectedImageIndex = ImageFolderClosed
                    };

                    repositoryNode.Nodes.Insert(0, superDomainsNode);
                }

                var superDomains = domain.DirectSuperDomains;
                superDomainsNode.Retain(Array.ConvertAll(superDomains, superDomain => new SuperDomainTag(repository, superDomain)));
                foreach (var superDomain in superDomains)
                {
                    if (!superDomain.IsAllorsUnitDomain)
                    {
                        var superDomainNode = superDomainsNode.Find(new SuperDomainTag(repository, superDomain));
                        if (superDomainNode == null)
                        {
                            superDomainNode = new TreeNode
                                {
                                    Text = superDomain.Name,
                                    Tag = new SuperDomainTag(repository, superDomain),
                                    ImageIndex = ImageTemplates,
                                    SelectedImageIndex = ImageTemplates
                                };

                            superDomainsNode.Nodes.Add(superDomainNode);
                        }
                        else
                        {
                            if (superDomain.Name == null || !superDomain.Name.Equals(superDomainNode.Text))
                            {
                                superDomainNode.Text = superDomain.Name;
                            }
                        }
                    }
                }
            }
            else
            {
                if (templatesNode != null)
                {
                    repositoryNode.Nodes.Remove(templatesNode);
                }

                if (superDomainsNode != null)
                {
                    repositoryNode.Nodes.Remove(superDomainsNode);
                }
            }

            var domains = domain.Domains;
            repositoryNode.Retain(Array.ConvertAll(domains, domein => new DomainTag(repository, domein)));
            foreach (var domein in domains)
            {
                if (!domein.IsAllorsUnitDomain)
                {
                    var treeNode = repositoryNode.Find(new DomainTag(repository,domein));

                    var text = domein.Name;
                    var imageIndex = !domein.IsSuperDomain ? ImageNamespace : ImageNamespaceLocked;
                    var selectedImageIndex = !domein.IsSuperDomain ? ImageNamespace : ImageNamespaceLocked;

                    if (treeNode == null)
                    {
                        treeNode = new TreeNode
                            {
                                Tag = new DomainTag(repository, domein),
                                Text = text, 
                                ImageIndex = imageIndex,
                                SelectedImageIndex = selectedImageIndex
                            };

                        repositoryNode.Nodes.Add(treeNode);
                    }
                    else
                    {
                        treeNode.SyncText(domein.Name);
                        treeNode.SyncImageIndex(imageIndex);
                        treeNode.SyncSelectedImageIndex(selectedImageIndex);
                    }

                    this.SyncObjectTypes(repository, treeNode, domein.DeclaredObjectTypes);
                }
            }
            
            var relationTypesWithoutObjectType = new List<RelationType>();
            foreach (var relationType in domain.RelationTypes)
            {
                if (!relationType.AssociationType.ExistObjectType)
                {
                    relationTypesWithoutObjectType.Add(relationType);
                }
            }

            this.SyncRelationTypes(repository, repositoryNode, relationTypesWithoutObjectType);

            if (syncRoot)
            {
                this.treeView.Sort();
                this.treeView.EndUpdate();
            }
        }

        private void SyncObjectTypes(Repository repository, TreeNode parentNode, ObjectType[] objectTypes)
        {
            parentNode.Retain(Array.ConvertAll(objectTypes, objectType => new ObjectTypeTag(repository, objectType)));

            foreach (var objectType in objectTypes)
            {
                var objectTypeTreeNode = parentNode.Find(new ObjectTypeTag(repository, objectType));

                var text = objectType.Name;
                var imageIndex = objectType.IsInterface
                                    ? (!objectType.DomainWhereDeclaredObjectType.IsSuperDomain ? ImageInterface : ImageInterfaceLocked)
                                    : (!objectType.DomainWhereDeclaredObjectType.IsSuperDomain ? ImageClass : ImageClassLocked);
                var selectedImageIndex = objectType.IsInterface
                                    ? (!objectType.DomainWhereDeclaredObjectType.IsSuperDomain ? ImageInterface : ImageInterfaceLocked)
                                    : (!objectType.DomainWhereDeclaredObjectType.IsSuperDomain ? ImageClass : ImageClassLocked);

                if (objectTypeTreeNode == null)
                {
                    objectTypeTreeNode = new TreeNode
                        {
                            Tag = new ObjectTypeTag(repository, objectType),
                            Text = text,
                            ImageIndex = imageIndex,
                            SelectedImageIndex = selectedImageIndex
                        };

                    var supertypesNode = new TreeNode
                    {
                        Text = TitleSupertypes,
                        Tag = new InheritancesTag(repository, objectType),
                        ImageIndex = ImageFolderClosed,
                        SelectedImageIndex = ImageFolderClosed
                    };

                    objectTypeTreeNode.Nodes.Add(supertypesNode);

                    parentNode.Nodes.Add(objectTypeTreeNode);
                }
                else
                {
                    objectTypeTreeNode.SyncText(text);
                    objectTypeTreeNode.SyncImageIndex(imageIndex);
                    objectTypeTreeNode.SyncSelectedImageIndex(selectedImageIndex);
                }

                var type = ((ObjectTypeTag)objectTypeTreeNode.Tag).ObjectType;
                var superTypesNode = objectTypeTreeNode.Find(new InheritancesTag(repository, type));

                // TODO: Sync instead of recreate
                superTypesNode.Nodes.Clear();

                var inheritances = type.InheritancesWhereSubtype;
                foreach (var inheritance in inheritances)
                {
                    var superType = inheritance.Supertype;
                    if (superType != null)
                    {
                        var superTypeNode = new TreeNode
                        {
                            Text = superType.Name,
                            Tag = new InheritanceTag(repository, objectType, inheritance),
                            ImageIndex =
                                superType.IsInterface
                                    ? (!inheritance.DomainWhereDeclaredInheritance.IsSuperDomain ? ImageInterface : ImageInterfaceLocked)
                                    : (!inheritance.DomainWhereDeclaredInheritance.IsSuperDomain ? ImageClass : ImageClassLocked),
                            SelectedImageIndex =
                                superType.IsInterface
                                    ? (!inheritance.DomainWhereDeclaredInheritance.IsSuperDomain ? ImageInterface : ImageInterfaceLocked)
                                    : (!inheritance.DomainWhereDeclaredInheritance.IsSuperDomain ? ImageClass : ImageClassLocked)
                        };

                        superTypesNode.Nodes.Add(superTypeNode);
                    }
                }

                this.SyncObjectType(repository, objectTypeTreeNode);
            }
        }

        private void SyncObjectType(Repository repository, TreeNode objectTypeTreeNode)
        {
            var objectType = ((ObjectTypeTag)objectTypeTreeNode.Tag).ObjectType;

            if (objectTypeTreeNode.IsExpanded)
            {
                var relationTypes = new List<RelationType>();
                foreach (var association in objectType.AssociationTypesWhereObjectType)
                {
                    relationTypes.Add(association.RelationTypeWhereAssociationType);
                }

                this.SyncRelationTypes(repository, objectTypeTreeNode, relationTypes);
            }
            else
            {
                objectTypeTreeNode.Retain(WinForms.Tag.EmptyRelationTypeList);
            }
        }

        private void SyncRelationTypes(Repository repository, TreeNode parentNode, List<RelationType> relationTypes)
        {
            parentNode.Retain(relationTypes.ConvertAll(relationType => new RelationTypeTag(repository, relationType)));
 
            foreach (var relationType in relationTypes)
            {
                var role = relationType.RoleType;

                var text = role.Name;
                var imageIndex = !relationType.DomainWhereDeclaredRelationType.IsSuperDomain ? ImageRole : ImageRoleLocked;
                var selectedImageIndex = !relationType.DomainWhereDeclaredRelationType.IsSuperDomain ? ImageRole : ImageRoleLocked;
                
                var relationTreeNode = parentNode.Find(new RelationTypeTag(repository, relationType));
                if (relationTreeNode == null)
                {
                    relationTreeNode = new TreeNode
                        {
                            Tag = new RelationTypeTag(repository, relationType),
                            Text = text, 
                            ImageIndex = imageIndex,
                            SelectedImageIndex = selectedImageIndex
                        };

                    parentNode.Nodes.Add(relationTreeNode);
                }
                else
                {
                    relationTreeNode.SyncText(text);
                    relationTreeNode.SyncImageIndex(imageIndex);
                    relationTreeNode.SyncSelectedImageIndex(selectedImageIndex);
                }
            }
        }
        #endregion

        #region Tree View Event Handlers
        private void TreeViewAfterCollapse(object sender, TreeViewEventArgs e)
        {
            var treeNode = e.Node;
            var objectTypeTag = treeNode.Tag as ObjectTypeTag;

            if (objectTypeTag != null)
            {
                var repository = objectTypeTag.Repository;
                this.SyncObjectType(repository, treeNode);
            }

            switch (treeNode.ImageIndex)
            {
                case ImageFolderClosed:
                case ImageFolderOpened:

                    treeNode.ImageIndex = ImageFolderClosed;
                    treeNode.SelectedImageIndex = ImageFolderClosed;
                    break;
            }
        }

        private void TreeViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            var treeNode = e.Node;
            var objectTypeTag = treeNode.Tag as ObjectTypeTag;

            if (objectTypeTag != null)
            {
                var repository = objectTypeTag.Repository;
                this.SyncObjectType(repository, treeNode);
            }

            switch (treeNode.ImageIndex)
            {
                case ImageFolderClosed:
                case ImageFolderOpened:

                    treeNode.ImageIndex = ImageFolderOpened;
                    treeNode.SelectedImageIndex = ImageFolderOpened;
                    break;
            }
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            this.ProcessSelection(e.Node);
        }

        private void TreeViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                this.DeleteSelection();
            }
        }

        private void TreeViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var p = new Point(e.X, e.Y);
                var node = this.treeView.GetNodeAt(p);

                this.treeView.SelectedNode = node;

                if (node == null)
                {
                    this.ProcessSelection(null);
                }
            }
        }
        #endregion

        #region Menu Item Event Handlers
        private void UpdateTemplateMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                this.templateSelection.Template.UpdateTemplate();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.StackTrace);
                MessageBox.Show("Could not update template\n\n" + e1.Message, "Allors Explorer - Update Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateMenuItemClick(object sender, EventArgs e)
        {
            Template[] allTemplates = { };

            if (this.templateSelection != null)
            {
                allTemplates = new[] { this.templateSelection.Template };
            }
            else if (this.repositorySelection != null)
            {
                allTemplates = this.repositorySelection.Repository.Templates.ToArray();
            }

            if (allTemplates.Length > 0)
            {
                var generatingDialog = new GeneratingDialog(allTemplates);
                generatingDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("No templates defined.", "Allors Explorer - No Templates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void OpenRepositoryMenuItemClick(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Filter = Repository.FileFilter };
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var directoryInfo = new FileInfo(openFileDialog.FileName).Directory;
                var repository = new Repository(directoryInfo);
                this.AddRepository(repository);
            }
        }

        private void AddTemplateMenuItemClick(object sender, EventArgs e)
        {
            var templateWizard = new AddTemplateWizard(this.templatesSelection.Repository);
            templateWizard.ShowDialog(this);
        }

        private void AddRepositoryMenuItemClick(object sender, EventArgs e)
        {
            var repositoryWizard = new AddRepositoryWizard();
            if (repositoryWizard.ShowDialog() == DialogResult.OK)
            {
                this.AddRepository(repositoryWizard.Repository);
            }
        }

        private void AddObjectTypeMenuItemClick(object sender, EventArgs e)
        {
            var typeWizard = new AddObjectTypeWizard(this.domainSelection.Domain);
            typeWizard.ShowDialog(this);
        }

        private void RemoveMenuItemClick(object sender, EventArgs e)
        {
            var inheritance = this.inheritanceSelection.Inheritance;

            if (MessageBox.Show(
                "Do you really want to remove " + inheritance.Supertype + "?",
                "Allors Explorer - Remove supertype",
                MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question).Equals(DialogResult.OK))
            {
                inheritance.Delete();
            }
        }

        private void AddRelationTypeMenuItemClick(object sender, EventArgs e)
        {
            var relationWizard = new AddRelationTypeWizard(this.objectTypeSelection.ObjectType);
            relationWizard.ShowDialog(this);
        }

        private void AddSuperDomainMenuItemClick(object sender, EventArgs args)
        {
            var openFileDialog = new OpenFileDialog { Filter = Repository.FileFilter };
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var directoryInfo = new FileInfo(openFileDialog.FileName).Directory;
                
                try
                {
                    this.superDomainsSelection.Repository.AddSuper(directoryInfo);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Allors Explorer - Add Super Domain Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CleanUpMenuItemClick(object sender, EventArgs e)
        {
            var domain = this.repositorySelection.Repository.Domain;

            // This purges all derived relations 
            // (forcing a a new derivation on next use).
            domain.Save(new XmlTextWriter(new StringWriter()));

            foreach (var objectType in domain.ObjectTypes)
            {
                objectType.SendChangedEvent();
            }

            foreach (var relationType in domain.RelationTypes)
            {
                relationType.SendChangedEvent();
            }

            foreach (var inheritance in domain.Inheritances)
            {
                if (!inheritance.ExistSubtype || !inheritance.ExistSupertype)
                {
                    inheritance.Delete();
                }
                else
                {
                    inheritance.SendChangedEvent();
                }
            }

            foreach (var template in this.repositorySelection.Repository.Templates)
            {
                template.Save();
            }

            domain.SendChangedEvent();
        }

        private void PullItemsUpMenuItemClick(object sender, EventArgs e)
        {
            if (this.objectTypeSelection != null)
            {
                var repository = this.objectTypeSelection.Repository;
                var pullObjectTypeUp = new PullUpObjectTypeWizard(repository, this.objectTypeSelection.ObjectType);
                pullObjectTypeUp.ShowDialog(this);
            }

            if (this.inheritanceSelection != null)
            {
                var repository = this.inheritanceSelection.Repository;
                var pullInheritanceUp = new PullUpInheritanceWizard(repository, this.inheritanceSelection.Inheritance);
                pullInheritanceUp.ShowDialog(this);
            }

            if (this.relationTypeSelection != null)
            {
                var repository = this.relationTypeSelection.Repository;
                var pullRelationTypeUp = new PullUpRelationTypeWizard(repository, this.relationTypeSelection.RelationType);
                pullRelationTypeUp.ShowDialog(this);
            }
        }

        private void PullItemsDownMenuItemClick(object sender, EventArgs e)
        {
            if (this.objectTypeSelection != null)
            {
                var repository = this.objectTypeSelection.Repository;
                var pushObjectTypeDown = new PushDownObjectTypeWizard(repository, this.objectTypeSelection.ObjectType);
                pushObjectTypeDown.ShowDialog(this);
            }

            if (this.inheritanceSelection != null)
            {
                var repository = this.inheritanceSelection.Repository;
                var pushInheritanceDown = new PushDownInheritanceWizard(repository, this.inheritanceSelection.Inheritance);
                pushInheritanceDown.ShowDialog(this);
            }

            if (this.relationTypeSelection != null)
            {
                var repository = this.relationTypeSelection.Repository;
                var pushRelationTypeDown = new PushDownRelationTypeWizard(repository, this.relationTypeSelection.RelationType);
                pushRelationTypeDown.ShowDialog(this);
            }
        }

        private void DeletMenuItemClick(object sender, EventArgs e)
        {
            this.DeleteSelection();
        }
        
        #endregion

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Explorer));
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.generateMenuItem = new System.Windows.Forms.MenuItem();
            this.openRepositoryMenuItem = new System.Windows.Forms.MenuItem();
            this.addRepositoryMenuItem = new System.Windows.Forms.MenuItem();
            this.addTemplateMenuItem = new System.Windows.Forms.MenuItem();
            this.addSuperDomainMenuItem = new System.Windows.Forms.MenuItem();
            this.addNamespaceMenuItem = new System.Windows.Forms.MenuItem();
            this.addTypeMenuItem = new System.Windows.Forms.MenuItem();
            this.removeMenuItem = new System.Windows.Forms.MenuItem();
            this.addRelationMenuItem = new System.Windows.Forms.MenuItem();
            this.updateTemplateMenuItem = new System.Windows.Forms.MenuItem();
            this.pullUpMenuItem = new System.Windows.Forms.MenuItem();
            this.pushDownMenuItem = new System.Windows.Forms.MenuItem();
            this.deleteMenuItem = new System.Windows.Forms.MenuItem();
            this.extraMenuItem = new System.Windows.Forms.MenuItem();
            this.cleanUpMenuItem = new System.Windows.Forms.MenuItem();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButton = new System.Windows.Forms.ToolBarButton();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.generateMenuItem,
            this.openRepositoryMenuItem,
            this.addRepositoryMenuItem,
            this.addTemplateMenuItem,
            this.addSuperDomainMenuItem,
            this.addNamespaceMenuItem,
            this.addTypeMenuItem,
            this.removeMenuItem,
            this.addRelationMenuItem,
            this.updateTemplateMenuItem,
            this.pullUpMenuItem,
            this.pushDownMenuItem,
            this.deleteMenuItem,
            this.extraMenuItem});
            // 
            // generateMenuItem
            // 
            this.generateMenuItem.Index = 0;
            this.generateMenuItem.Text = "Generate";
            this.generateMenuItem.Click += new System.EventHandler(this.GenerateMenuItemClick);
            // 
            // openRepositoryMenuItem
            // 
            this.openRepositoryMenuItem.Index = 1;
            this.openRepositoryMenuItem.Text = "Open Repository";
            this.openRepositoryMenuItem.Click += new System.EventHandler(this.OpenRepositoryMenuItemClick);
            // 
            // addRepositoryMenuItem
            // 
            this.addRepositoryMenuItem.Index = 2;
            this.addRepositoryMenuItem.Text = "Add Repository";
            this.addRepositoryMenuItem.Click += new System.EventHandler(this.AddRepositoryMenuItemClick);
            // 
            // addTemplateMenuItem
            // 
            this.addTemplateMenuItem.Index = 3;
            this.addTemplateMenuItem.Text = "Add Template";
            this.addTemplateMenuItem.Click += new System.EventHandler(this.AddTemplateMenuItemClick);
            // 
            // addSuperDomainMenuItem
            // 
            this.addSuperDomainMenuItem.Index = 4;
            this.addSuperDomainMenuItem.Text = "Add Super Domain";
            this.addSuperDomainMenuItem.Click += new System.EventHandler(this.AddSuperDomainMenuItemClick);
            // 
            // addTypeMenuItem
            // 
            this.addTypeMenuItem.Index = 6;
            this.addTypeMenuItem.Text = "Add Object Type";
            this.addTypeMenuItem.Click += new System.EventHandler(this.AddObjectTypeMenuItemClick);
            // 
            // removeMenuItem
            // 
            this.removeMenuItem.Index = 7;
            this.removeMenuItem.Text = "Remove";
            this.removeMenuItem.Click += new System.EventHandler(this.RemoveMenuItemClick);
            // 
            // addRelationMenuItem
            // 
            this.addRelationMenuItem.Index = 8;
            this.addRelationMenuItem.Text = "Add Relation Type";
            this.addRelationMenuItem.Click += new System.EventHandler(this.AddRelationTypeMenuItemClick);
            // 
            // updateTemplateMenuItem
            // 
            this.updateTemplateMenuItem.Index = 9;
            this.updateTemplateMenuItem.Text = "Update Template";
            this.updateTemplateMenuItem.Click += new System.EventHandler(this.UpdateTemplateMenuItemClick);
            // 
            // pullUpMenuItem
            // 
            this.pullUpMenuItem.Index = 10;
            this.pullUpMenuItem.Text = "Pull Up";
            this.pullUpMenuItem.Click += new System.EventHandler(this.PullItemsUpMenuItemClick);
            // 
            // pushDownMenuItem
            // 
            this.pushDownMenuItem.Index = 11;
            this.pushDownMenuItem.Text = "Push Down";
            this.pushDownMenuItem.Click += new System.EventHandler(this.PullItemsDownMenuItemClick);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Index = 12;
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.DeletMenuItemClick);
            // 
            // extraMenuItem
            // 
            this.extraMenuItem.Index = 13;
            this.extraMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cleanUpMenuItem});
            this.extraMenuItem.Text = "Extra";
            // 
            // cleanUpMenuItem
            // 
            this.cleanUpMenuItem.Index = 0;
            this.cleanUpMenuItem.Text = "Clean Up";
            this.cleanUpMenuItem.Click += new System.EventHandler(this.CleanUpMenuItemClick);
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.ContextMenu = this.contextMenu;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 24);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(304, 344);
            this.treeView.TabIndex = 0;
            this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterCollapse);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeViewKeyDown);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeViewMouseDown);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "method.ico");
            this.imageList.Images.SetKeyName(8, "method-locked.ico");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "namespace-locked.ico");
            this.imageList.Images.SetKeyName(14, "class-locked.ico");
            this.imageList.Images.SetKeyName(15, "interface-locked.ico");
            this.imageList.Images.SetKeyName(16, "role-locked.ico");
            // 
            // toolBar
            // 
            this.toolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton});
            this.toolBar.ButtonSize = new System.Drawing.Size(16, 16);
            this.toolBar.Divider = false;
            this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(304, 26);
            this.toolBar.TabIndex = 1;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBarButtonClick);
            // 
            // toolBarButton
            // 
            this.toolBarButton.ImageIndex = 1;
            this.toolBarButton.Name = "toolBarButton";
            this.toolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            // 
            // Explorer
            // 
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolBar);
            this.Name = "Explorer";
            this.Size = new System.Drawing.Size(304, 368);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button.Equals(this.toolBarButton))
            {
                this.SyncRepositories();
            }

            this.treeView.Sort();
        }

        private void OnMetaObjectChanged(object sender, RepositoryMetaObjectChangedEventArgs args)
        {
            var changedDomain = args.MetaObject as Domain;
            if (changedDomain != null)
            {
                this.SyncRepository(args.Repository);
            }

            var changedObjectType = args.MetaObject as ObjectType;
            if (changedObjectType != null)
            {
                this.SyncRepository(args.Repository);
            }

            var changedInheritance = args.MetaObject as Inheritance;
            if (changedInheritance != null)
            {
                this.SyncRepository(args.Repository);
            }

            var changedRelationType = args.MetaObject as RelationType;
            if (changedRelationType != null)
            {
                this.SyncRepository(args.Repository);
            }

            var metaObjectChanged = this.MetaObjectChanged;
            if (metaObjectChanged != null)
            {
                metaObjectChanged(this, args);
            }
        }

        private void OnMetaObjectDeleted(object sender, RepositoryMetaObjectDeletedEventArgs args)
        {
            this.SyncRepository(args.Repository);

            var metaObjectDeleted = this.MetaObjectDeleted;
            if (metaObjectDeleted != null)
            {
                metaObjectDeleted(this, args);
            }
        }

        private void OnObjectChanged(object sender, RepositoryObjectChangedEventArgs args)
        {
            this.SyncRepository(args.Repository);

            var repositoryObjectChanged = this.RepositoryObjectChanged;
            if (repositoryObjectChanged != null)
            {
                repositoryObjectChanged(this, args);
            }
        }

        private void OnObjectDeleted(object sender, RepositoryObjectDeletedEventArgs args)
        {
            this.SyncRepository(args.Repository);

            var repositoryObjectDeleted = this.RepositoryObjectDeleted;
            if (repositoryObjectDeleted != null)
            {
                repositoryObjectDeleted(this, args);
            }
        }
    }
}