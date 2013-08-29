using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Golem.Framework
{
    public static class JavascriptFunctions
    {
        public static string DisableMouseOverEvents = "document.mouseover = function() { return false; };document.mouseout = function() { return false; };return;";
        public static string DisableAllClickEvents = "document.onclick = function() { return false; };"; 
        public static string HighlightOnMouseOver = "document.addEventListener('mouseover', function(e) { e = e || window.event; window.lastBorder=e.target.style.border; e.target.style.border='3px solid blue'}, false); document.addEventListener('mouseout', function(e) { e = e || window.event; e.target.style.border=''}, false);return;";

    }
}
