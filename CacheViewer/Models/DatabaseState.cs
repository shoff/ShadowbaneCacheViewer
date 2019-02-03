namespace CacheViewer.Models
{
    using System;
    using Domain.Extensions;

    public class DatabaseState
    {
        private bool texturesSaved;
        private bool meshesSaved;
        private bool renderSaved;
        private bool cObjectSaved;
        private bool renderOffsetSaved;
        private bool texturesAssociated;
        public event EventHandler StateChanged;

        public bool TexturesSaved
        {
            get => this.texturesSaved;
            set
            {
                this.texturesSaved = value;
                StateChanged.Raise(this, EventArgs.Empty);
            }
        }

        public bool MeshesSaved
        {
            get => this.meshesSaved;
            set
            {
                this.texturesSaved = value;
                StateChanged.Raise(this, EventArgs.Empty);
            }
        }

        public bool RenderSaved
        {
            get => this.renderSaved;
            set
            {
                this.texturesSaved = value;
                StateChanged.Raise(this, EventArgs.Empty);
            }
        }

        public bool CObjectSaved
        {
            get => this.cObjectSaved;
            set
            {
                this.texturesSaved = value;
                StateChanged.Raise(this, EventArgs.Empty);
            }
        }

        public bool RenderOffsetSaved
        {
            get => this.renderOffsetSaved;
            set
            {
                this.texturesSaved = value;
                StateChanged.Raise(this, EventArgs.Empty);
            }
        }

        public bool TexturesAssociated
        {
            get => this.texturesAssociated;
            set
            {
                this.texturesSaved = value;
                StateChanged.Raise(this, EventArgs.Empty);
            }
        }
    }
}