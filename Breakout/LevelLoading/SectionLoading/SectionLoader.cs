namespace Breakout.LevelLoading.SectionLoading {
    public abstract class SectionLoader {
        /// <summary>
        /// How to process each line
        /// </summary>
        protected abstract void ProcessSectionLine(string line);


        /// <summary>
        /// Load text from section into internal fields
        /// </summary>
        public abstract void LoadSection();
    }
}