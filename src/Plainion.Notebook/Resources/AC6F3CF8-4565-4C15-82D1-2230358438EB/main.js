$(function () {
	var elements = $('#pages option').map(function () {
		return this.value;
	}).get();

	$('#edit-pagetext').textcomplete([
		{
			match: /\[(\w*)$/,
			search: function (term, callback) {
				callback($.map(elements, function (element) {
					return element.indexOf(term) === 0 ? element : null;
				}));
			},
			index: 1,
			replace: function (element) {
				return ['[' + element + ']', ''];
			}
		}
	]);
});
