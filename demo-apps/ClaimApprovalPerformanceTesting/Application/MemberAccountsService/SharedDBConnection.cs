using System.Threading;

namespace HP.SOAQ.ServiceSimulation.Demo {

    /**
     * This class simulates performance impact of a single shared resource (i.e. the single DB connection)
     * to the service performance. It has a fixed processing time so the response time of the overall transaction
     * will depend on the number of concurrent calls being served.
     **/
    class SharedDBConnection {

        private static readonly int PROCESSING_TIME = 100;

        public static readonly SharedDBConnection INSTANCE = new SharedDBConnection();

        public static SharedDBConnection getInstance() {
            return INSTANCE;
        }

        public void process() {
            lock(INSTANCE) {
                Thread.Sleep(PROCESSING_TIME);
            }
        }

    }
}
