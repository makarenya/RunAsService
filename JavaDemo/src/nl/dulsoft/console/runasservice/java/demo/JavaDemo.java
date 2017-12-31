/*
 * JavaDemo, demonstrates the use of RunAsService, the service to run non-
 * service applications as a service, using a Java application
 *
 * Distributable under Academic Free License Version 1.2
 * See http://www.opensource.org/licenses/academic.php
 */
package nl.dulsoft.console.runasservice.java.demo;

import java.util.Timer;
import java.util.TimerTask;
import java.util.Date;
import java.io.IOException;

import org.apache.log4j.Logger;
import org.apache.log4j.Layout;
import org.apache.log4j.PatternLayout;
import org.apache.log4j.Appender;
import org.apache.log4j.FileAppender;
import org.apache.log4j.Level;

/**
 * <p>JavaDemo, demonstrates the use of RunAsService, the service to run non-
 * service applications as a service, using a Java application.</p>
 *
 * <p>Distributable under Academic Free License Version 1.2
 * <a href="http://www.opensource.org/licenses/academic.php">
 * See http://www.opensource.org/licenses/academic.php</a>.</p>
 *
 * @author Marcel Dullaart
 */
public final class JavaDemo {
    /** Scheduler. */
    private Timer timer;
    /** Class level logger. */
    private static final Logger logger = Logger.getLogger(JavaDemo.class);
    /** Scheduled interval. */
    private static final long interval = 60000;

    /**
     * Applications main entry point.
     * @param args supplied at the command line
     * @throws IOException in case system input cannot be read
     */
    public static void main(final String[] args) throws IOException {
        Layout layout = new PatternLayout("%m%n");
        Appender app = new FileAppender(layout, "JavaDemo.log");
        logger.addAppender(app);
        logger.setLevel(Level.INFO);

        JavaDemo demo = new JavaDemo();
        demo.run();
    }

    /**
     * Runs the application, by starting the timer and waiting fo a stop input.
     * @throws IOException in case system input cannot be read
     */
    private void run() throws IOException {
        if (logger.isDebugEnabled()) {
            logger.debug("+run");
        }

        timer = new Timer();
        TimerTask task = new DemoTimerTask();

        timer.schedule(task, 0, interval);

        System.out.println("Press q to end");
        while (System.in.read() != 'q') {
            ;// Intentionally left empty
        }

        timer.cancel();

        if (logger.isDebugEnabled()) {
            logger.debug("+run");
        }
    }

    /**
     * Task executed by the timer at a regular interval.
     */
    static final class DemoTimerTask extends TimerTask {
        /**
         * Method run each interval/
         */
        public void run() {
            Date date = new Date();
            logger.info("Timed event occured. Current time is: " + date);
        }
    }
}
