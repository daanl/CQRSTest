﻿using System;

namespace CqrsTests.Infrastructure.Events
{
    public abstract class VersionedEvent : IVersionedEvent
    {
        public Guid SourceId { get; set; }

        public int Version { get; set; }
    }
}