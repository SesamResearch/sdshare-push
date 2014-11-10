
package no.bouvet.sesam.sdshare;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.slf4j.Marker;

public class SLF4JLogger implements Logger {
    static Logger log = LoggerFactory.getLogger(SLF4JLogger.class.getName());

    //trace level

    @Override
    public void trace(Marker marker, String msg) { log.trace(marker, msg); }

    @Override
    public void trace(Marker marker, String format, Object arg) { log.trace(marker, format, arg); }

    @Override
    public void trace(Marker marker, String format, Object arg1, Object arg2) { log.trace(marker, format, arg1, arg2); }

    @Override
    public void trace(Marker marker, String format, Object... argArray) { log.trace(marker, format, argArray); }

    @Override
    public void trace(Marker marker, String msg, Throwable t) { log.trace(msg, t); }

    @Override
    public void trace(String msg) { log.trace(msg); }

    @Override
    public void trace(String format, Object arg) { log.trace(format, arg); }

    @Override
    public void trace(String format, Object arg1, Object arg2) { log.trace(format, arg1, arg2); }

    @Override
    public void trace(String format, Object... arguments) { log.trace(format, arguments); }

    @Override
    public void trace(String msg, Throwable t) { log.trace(msg, t); }
    // debug level

    @Override
    public void debug(Marker marker, String msg) { log.debug(marker, msg); }

    @Override
    public void debug(Marker marker, String format, Object arg) { log.debug(marker, format, arg); }

    @Override
    public void debug(Marker marker, String format, Object arg1, Object arg2) { log.debug(marker, format, arg1, arg2); }

    @Override
    public void debug(Marker marker, String format, Object... argArray) { log.debug(marker, format, argArray); }

    @Override
    public void debug(Marker marker, String msg, Throwable t) { log.debug(marker, msg, t); }

    @Override
    public void debug(String msg) { log.debug(msg); }

    @Override
    public void debug(String format, Object arg) { log.debug(format, arg); }

    @Override
    public void debug(String format, Object arg1, Object arg2) { log.debug(format, arg1, arg2); }

    @Override
    public void debug(String format, Object... arguments) { log.debug(format, arguments); }

    @Override
    public void debug(String msg, Throwable t) { log.debug(msg, t); }

    // info level


    @Override
    public void info(Marker marker, String msg) { log.info(marker, msg); }

    @Override
    public void info(Marker marker, String format, Object arg) { log.info(marker, format, arg); }

    @Override
    public void info(Marker marker, String format, Object arg1, Object arg2) { log.info(marker, format, arg1, arg2); }

    @Override
    public void info(Marker marker, String format, Object... argArray) { log.info(marker, format, argArray); }

    @Override
    public void info(Marker marker, String msg, Throwable t) { log.info(marker, msg, t); }

    @Override
    public void info(String msg) { log.info(msg); }

    @Override
    public void info(String format, Object arg) { log.info(format, arg); }

    @Override
    public void info(String format, Object arg1, Object arg2) { log.info(format, arg1, arg2); }

    @Override
    public void info(String format, Object... arguments) { log.info(format, arguments); }

    @Override
    public void info(String msg, Throwable t) { log.info(msg, t); }

    // warn level

    @Override
    public void warn(Marker marker, String msg) { log.warn(marker, msg); }

    @Override
    public void warn(Marker marker, String format, Object arg) { log.warn(marker, format, arg); }

    @Override
    public void warn(Marker marker, String format, Object arg1, Object arg2) { log.warn(marker, format, arg1, arg2); }

    @Override
    public void warn(Marker marker, String format, Object... argArray) { log.warn(marker, format, argArray); }

    @Override
    public void warn(Marker marker, String msg, Throwable t) { log.warn(marker, msg, t); }

    @Override
    public void warn(String msg) { log.warn(msg); }

    @Override
    public void warn(String format, Object arg) { log.warn(format, arg); }

    @Override
    public void warn(String format, Object arg1, Object arg2) { log.warn(format, arg1, arg2); }

    @Override
    public void warn(String format, Object... arguments) { log.warn(format, arguments); }

    @Override
    public void warn(String msg, Throwable t) { log.warn(msg, t); }

    // error level


    @Override
    public void error(Marker marker, String msg) { log.error(marker, msg); }

    @Override
    public void error(Marker marker, String format, Object arg) { log.error(marker, format, arg); }

    @Override
    public void error(Marker marker, String format, Object arg1, Object arg2) { log.error(marker, format, arg1, arg2); }

    @Override
    public void error(Marker marker, String format, Object... argArray) { log.error(marker, format, argArray); }

    @Override
    public void error(Marker marker, String msg, Throwable t) { log.error(marker, msg, t); }

    @Override
    public void error(String msg) { log.error(msg); }

    @Override
    public void error(String format, Object arg) { log.error(format, arg); }

    @Override
    public void error(String format, Object arg1, Object arg2) { log.error(format, arg1, arg2); }

    @Override
    public void error(String format, Object... arguments) { log.error(format, arguments); }

    @Override
    public void error(String msg, Throwable t) { log.error(msg, t); }

    // other funcs
    @Override
    public String getName() {
        return "Name wasn't changed";
    }

    @Override
    public boolean isDebugEnabled() {  return log.isDebugEnabled(); }

    @Override
    public boolean isDebugEnabled(Marker marker) {  return log.isDebugEnabled(marker); }

    @Override
    public boolean isErrorEnabled() {  return log.isErrorEnabled(); }

    @Override
    public boolean isErrorEnabled(Marker marker) {  return log.isErrorEnabled(marker); }

    @Override
    public boolean isInfoEnabled() {  return log.isInfoEnabled(); }

    @Override
    public boolean isInfoEnabled(Marker marker) {  return log.isInfoEnabled(marker); }

    @Override
    public boolean isTraceEnabled() {  return log.isTraceEnabled(); }

    @Override
    public boolean isTraceEnabled(Marker marker) {  return log.isTraceEnabled(marker); }

    @Override
    public boolean isWarnEnabled() {  return log.isWarnEnabled(); }

    @Override
    public boolean isWarnEnabled(Marker marker) {  return log.isWarnEnabled(marker); }


}
