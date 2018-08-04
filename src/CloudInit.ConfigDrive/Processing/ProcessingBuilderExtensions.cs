﻿namespace Contiva.CloudInit.ConfigDrive.Processing
{
    public static class ProcessingBuilderExtensions
    {
        public static ProcessorBuilder Processing(this IProcessableBuilder builder)
        {
            return new ProcessorBuilder(builder) ;
        }
    }

}