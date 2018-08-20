$(document).ready(function () {
    $('#nestable').nestable({
        group: 1
        , onDragFinished: function (currentNode, parentNode) {
			
			if(parentNode.parentNode.parentNode.className == "dd-item pagina")
			{
				return false
			}
			
            if (currentNode.className == "dd-item pagina" && parentNode.parentNode.parentNode.tagName == "DIV") {
                return false;
            }
			if (currentNode.className == "dd-item pagina" && parentNode.parentNode.parentNode.className == "dd-item pagina") {
                return false;
            }
            return true;
        }
    });
});