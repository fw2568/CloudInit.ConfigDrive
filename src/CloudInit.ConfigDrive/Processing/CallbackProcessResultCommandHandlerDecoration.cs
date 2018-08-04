﻿using System;
using System.IO;
using Contiva.Windows.ImagingApi;

namespace Contiva.CloudInit.ConfigDrive.Processing
{
    internal class CallbackProcessResultCommandHandlerDecoration : ICommandHandler<ProcessResultCommand>
    {
        public Action<ConfigDriveContent> ContentAction { get; set; }
        private readonly ICommandHandler<ProcessResultCommand> _decoratedHandler;

        public CallbackProcessResultCommandHandlerDecoration(ICommandHandler<ProcessResultCommand> decoratedHandler)
        {
            _decoratedHandler = decoratedHandler;
        }

        public void HandleCommand(ProcessResultCommand command)
        {
            ContentAction?.Invoke(command.Content);
            _decoratedHandler.HandleCommand(command);
        }
    }

    internal class ImageFileProcessResultCommandHandlerDecoration : ICommandHandler<ProcessResultCommand>
    {
        public string Filename { get; set; }
        private readonly ICommandHandler<ProcessResultCommand> _decoratedHandler;

        public ImageFileProcessResultCommandHandlerDecoration(ICommandHandler<ProcessResultCommand> decoratedHandler)
        {
            _decoratedHandler = decoratedHandler;
        }

        public void HandleCommand(ProcessResultCommand command)
        {
            var isoNuilder = new IsoImageBuilder(FileSystemType.Joliet).VolumeName(command.Content.MediaName);

            foreach (var contentFile in command.Content.Files)
            {
                isoNuilder.AddFile(contentFile.Path, contentFile.Content);
            }

            using (var isoStream = isoNuilder.Build())
            {
                using (var fileStream = File.OpenWrite(Filename))
                {
                    isoStream.CopyTo(fileStream);
                    fileStream.Close();
                }
            }

            _decoratedHandler.HandleCommand(command);
        }
    }
}
