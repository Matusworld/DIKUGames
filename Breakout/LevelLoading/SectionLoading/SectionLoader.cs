namespace Breakout.LevelLoading.SectionLoading {
    /// <summary>
    /// Facilitate the loading, i.e. processing and storage, of a section.
    /// Must be subtyped to a section.
    /// </summary>
    public abstract class SectionLoader {
        protected SectionStreamReader reader;
        protected string section;

        public SectionLoader(SectionStreamReader reader) {
            this.reader = reader;
        }

        /// <summary>
        /// Processing of one line.
        /// </summary>
        /// <param name="line"> the line coming from section of ASCII file.</param>
        protected abstract void ProcessSectionLine(string line);

        /// <summary>
        /// Load text from section into internal fields. 
        /// Assume reader is reset prior to loading.
        /// </summary>
        public virtual void LoadSection(string path) {
            reader.SetPath(path);
            reader.SetSection(section);

            string line;
            
            while ((line = reader.ReadSectionLine()) != null) {
                ProcessSectionLine(line);
            }

            reader.Reset();
        }

        /// <summary>
        /// Clear all previously loaded data.
        /// </summary>
        public abstract void ClearLoader();
    }
}