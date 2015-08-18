using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Meridian59.Common.Interfaces;
using Meridian59.Data.Lists;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.GameMessages;

namespace Meridian59.Data.Models.AdminData
{
    public class AdminData : INotifyPropertyChanged, IClearable
    {
        //Handles the objects we're watching in the AdminUI
        private BaseList<AdminObject> TrackedObjects { get; set; }
        public void TrackAdminObject(AdminObject adminObject)
        {
            TrackedObjects.Add(adminObject);
            OnTrackedAdminObjectAdded(new AddTrackedAdminObjectEventHandlerArgs(this, adminObject));
        }
        public void UntrackAdminObject(AdminObject adminObject)
        {
            TrackedObjects.Remove(adminObject);
        }

        public AdminData()
        {
            TrackedObjects = new BaseList<AdminObject>();
        }

        /// <summary>
        /// Interface to the Client's Network
        /// </summary>
        public event GameMessageEventHandler PacketSend;

        #region events
        //Event to add an object 
        public delegate void AddTrackedAdminObjectEventHandler(object sender, AddTrackedAdminObjectEventHandlerArgs args);
        public event AddTrackedAdminObjectEventHandler AdminObjectAdded;

        protected void OnTrackedAdminObjectAdded(AddTrackedAdminObjectEventHandlerArgs e)
        {
            if (AdminObjectAdded != null) AdminObjectAdded(this, e);
        }

        //Event to pass a message back to the console
        public delegate void LogAdminMessageEventHandler(LogAdminMessageEventHandlerArgs e);
        public event LogAdminMessageEventHandler LogAdminMessage;
        protected void OnLogAdminMessage(LogAdminMessageEventHandlerArgs e)
        {
            if (LogAdminMessage != null)
            {
                LogAdminMessage(e);
            }
        }
        #endregion

        void adminObjectProperty_ValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender.GetType() == typeof (AdminObjectProperty))
            {
                AdminObjectProperty changedProperty = (AdminObjectProperty)sender;
                AdminObject changedAdminObject = changedProperty.GetOwner();

                if (PacketSend != null)
                {
                    PacketSend(this,new GameMessageEventArgs(new ReqAdminMessage(string.Format("set object {0} {1} {2} {3}",
                        changedAdminObject.ObjectNumber, changedProperty.PropertyName, changedProperty.PropertyType, changedProperty.PropertyValue))));
                    PacketSend(this, new GameMessageEventArgs(new ReqAdminMessage(String.Format("show object {0}", changedAdminObject.ObjectNumber))));
                }

            }
            else
            {
                throw new Exception("What?");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            TrackedObjects.Clear();
        }
        #endregion

        public void HandleAdminMessage(AdminMessage message)
        {
            if (message.Message.StartsWith(":< OBJECT"))
            {
                HandleAdminShowObjectMessage(message);
            }
            OnLogAdminMessage(new LogAdminMessageEventHandlerArgs(message.Message));
        }
        public void HandleAdminShowObjectMessage(AdminMessage message)
        {
            AdminObject obj = null;
            bool track = false;

            //Look for object info
            var regex = new Regex(@":< OBJECT (?<objectnumber>\d*) is CLASS (?<classname>.*)");
            var matches = regex.Matches(message.Message);
            if (matches.Count != 1)
            {
                throw new Exception("Improper Response in HandleAdminShowObjectMessage()");
            }

            string classname = matches[0].Groups["classname"].ToString();
            int objectnumber = Convert.ToInt32(matches[0].Groups["objectnumber"].ToString());

            if (TrackedObjects.Any(o => o.ObjectNumber == objectnumber))
            {
                obj = TrackedObjects.First(o => o.ObjectNumber == objectnumber);
            }
            else
            {
                obj = new AdminObject(classname, objectnumber);
                track = true;
            }

            regex = new Regex(@": (?<property>\w*)\s*=\s(?<datatype>[\w$]*)\s(?<value>\w*)");
            if (regex.IsMatch((message.Message)))
            {
                BaseList<AdminObjectProperty> props = new BaseList<AdminObjectProperty>();
                matches = regex.Matches(message.Message);
                foreach (Match match in matches)
                {
                    props.Add(new AdminObjectProperty(match.Groups["property"].ToString(), match.Groups["datatype"].ToString(), match.Groups["value"].ToString(), obj, adminObjectProperty_ValueChanged));
                }
                if (obj != null)
                {
                    obj.SetProperties(props);
                }
            }

            if (track)
            {
                TrackAdminObject(obj);
            }
        }

    }
}
