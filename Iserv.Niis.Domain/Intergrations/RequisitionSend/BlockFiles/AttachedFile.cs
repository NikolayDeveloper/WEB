namespace Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockFiles
{
    public class AttachedFile
    {
        /// <remarks/>
        public BlockFileAttachedFileType Type { get; set; }

        /// <remarks/>
        public string PageCount { get; set; }

        /// <remarks/>
        public BlockFileAttachedFileFile File { get; set; }
    }
}