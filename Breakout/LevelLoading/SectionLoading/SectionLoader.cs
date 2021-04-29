namespace Breakout.LevelLoading.SectionLoading {
    public abstract class SectionLoader {
        /// <summary>
        /// Processing of a line
        /// </summary>
        /// <param name="line"> the line coming from section of ASCII file</param>
        protected abstract void ProcessSectionLine(string line);


        /// <summary>
        /// Load text from section into internal fields
        /// </summary>
        public abstract void LoadSection();
    }
}