using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public sealed class Messages
    {
        private readonly Dictionary<Type, List<object>> _msgActions = new Dictionary<Type, List<object>>();
        private readonly Dictionary<object, List<object>> _ownerMsgActions = new Dictionary<object, List<object>>();
        
        public void Publish<Message>(Message payload)
        {
            var msgType = payload.GetType();
            if (_msgActions.ContainsKey(msgType))
                _msgActions[msgType].ForEach(a => ((Action<Message>)a).Invoke(payload));
        }

        public void Subscribe<Message>(Action<Message> onMsg, object owner)
        {
            var topic = typeof(Message);
            if (!_msgActions.ContainsKey(topic))
                _msgActions[topic] = new List<object>();
            if (!_ownerMsgActions.ContainsKey(owner))
                _ownerMsgActions[owner] = new List<object>();
            _msgActions[topic].Add(onMsg);
            _ownerMsgActions[owner].Add(onMsg);
        }

        public void Unsubscribe(object owner)
        {
            if (!_ownerMsgActions.ContainsKey(owner))
                return;
            var ownedMsgActions = _ownerMsgActions[owner].ToList();
            _msgActions.ForEach(e => e.Value.RemoveAll(a => ownedMsgActions.Contains(a)));
            _ownerMsgActions.Remove(owner);
        }
    }
}
