namespace Breakout.LevelLoading.SectionLoading {
    public abstract class SectionLoader {
        protected SectionStreamReader reader;

        public SectionLoader(SectionStreamReader reader) {
            this.reader = reader;
        }

        /// <summary>
        /// Processing of a line
        /// </summary>
        /// <param name="line"> the line coming from section of ASCII file</param>
        protected abstract void ProcessSectionLine(string line);

        /// <summary>
        /// Load text from section into internal fields. 
        /// Assume reader is reset prior to loading.
        /// </summary>
        public abstract void LoadSection(string path);

        /// <summary>
        /// Clear all previously loaded data
        /// </summary>
        public abstract void ClearLoader();
    }
}