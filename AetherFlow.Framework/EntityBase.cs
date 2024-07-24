using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using Microsoft.Xrm.Sdk.Messages;

namespace AetherFlow.Framework
{
    public abstract class EntityBase : EntityTracker
    {
        protected readonly IOrganizationService Service;
        protected bool IsPreOperation = false;
        protected Entity TargetEntity = null;

        /// <summary>
        ///     Constructor used to set up an existing entity
        /// </summary>
        /// <param name="logicalName"></param>
        /// <param name="record"></param>
        /// <param name="service"></param>
        protected EntityBase(string logicalName, Entity record, IOrganizationService service)
            : base(logicalName, record.Id, record.Attributes)
        {
            Service = service;

            if (logicalName != record.LogicalName)
                throw new Exception("An invalid entity type was provided.");
        }

        /// <summary>
        ///     Constructor for a new entity.
        /// </summary>
        /// <param name="logicalName"></param>
        /// <param name="service"></param>
        protected EntityBase(string logicalName, IOrganizationService service = null)
            : base(logicalName)
        {
            Service = service;
        }

        /// <summary>
        ///     Constructor for existing entity - however, get no attributes!
        /// </summary>
        /// <param name="logicalName"></param>
        /// <param name="id"></param>
        /// <param name="service"></param>
        protected EntityBase(string logicalName, Guid id, IOrganizationService service)
            : base(logicalName, id)
        {
            Service = service;
        }

        public void RegisterAsPreOperation(Entity target)
        {
            TargetEntity = target;
            IsPreOperation = true;
        }

        /// <summary>
        ///     Save the record - be this a Create or Update, only changes sent
        ///     will be pushed to the server.
        /// </summary>
        public void Save()
        {
            // Validate service object
            if (Service == null)
                throw new Exception("Unable to save an entity object without a service object");

            if (IsPreOperation && TargetEntity != null)
            {
                foreach (var c in Changes) 
                    TargetEntity[c.Key] = c.Value;
                Changes = new Dictionary<string, object>();
                return;
            }

            var obj = GetUpdatedEntity(true);
            if (obj == null) return;
            if (EntityGuid == null) EntityGuid = Service.Create(obj);
            else Service.Update(obj);
        }

        public void Delete()
        {
            // Check the id 
            if (EntityGuid != null)
                Service.Delete(LogicalName, (Guid)EntityGuid);
        }

        /// <summary>
        ///     Get multiple attributes on the current entity.
        /// </summary>
        /// <param name="attributes">String array of attributes</param>
        public EntityBase Get(string[] attributes)
        {
            // If we have no id, do nothing
            if (EntityGuid == null)
                return null;

            // Get the attributes;
            Entity entity;
            try
            {
                entity = Service.Retrieve(
                    LogicalName,
                    (Guid)EntityGuid,
                    new ColumnSet(attributes)
                );
            }
            catch
            {
                return null;
            }

            // Foreach attribute, add to the entity;
            foreach (var attribute in attributes)
                if (Values.ContainsKey(attribute))
                    Values[attribute] =
                        entity.Contains(attribute)
                            ? entity[attribute]
                            : null;
                else
                    Values.Add(
                        attribute,
                        entity.Contains(attribute)
                            ? entity[attribute]
                            : null
                    );

            // We have updated the entity.. 
            // Remove all changes relating to the fields updated
            var tmp = Changes;
            Changes = new Dictionary<string, object>();
            foreach (var change in tmp.Where(change => !attributes.Contains(change.Key)))
                Changes.Add(change);

            // Finally return this object
            return this;
        }

        /// <summary>
        ///     Returns a delete request for the current object
        /// </summary>
        /// <returns></returns>
        public DeleteRequest DeleteRequest()
        {
            if (EntityGuid == null) return null;

            return new DeleteRequest {
                Target = GetReference()
            };
        }

        /// <summary>
        ///     Returns an update request for the current object
        /// </summary>
        /// <returns></returns>
        public UpdateRequest UpdateRequest()
        {
            if (EntityGuid == null) return null;
            if (Changes.Count == 0) return null;

            // Return the update, this can then be bulk updated
            return new UpdateRequest { Target = GetUpdatedEntity() };
        }

        /// <summary>
        ///     Returns a create request for the current object
        /// </summary>
        /// <returns></returns>
        public CreateRequest CreateRequest()
        {
            if (EntityGuid != null) return null;

            // Return the create object, this can then be bulk updated
            return new CreateRequest { Target = GetUpdatedEntity() };
        }
    }
}