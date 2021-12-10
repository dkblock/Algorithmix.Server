let duration = 500;
let rootChanged = false;

const tree = d3.tree().separation(() => 40);
const svg = d3.select('#svg');
const g = svg.append('g').attr('transform', 'translate(50, 50)');
const gLinks = g.append('g');
const gNodes = g.append('g');

const oldPos = {};
const updateTree = () => {
  const root = d3.hierarchy(data);
  const newTreeSize = [root.descendants().length * 40, ((root.height + 1) * 2 - 1) * 30];

  if (tree.size()[0] !== newTreeSize[0] || tree.size()[1] !== newTreeSize[1]) {
    tree.size(newTreeSize);
  }

  tree(root);

  const nodes = root.descendants().filter((d) => d.data.data !== null);
  const link = gLinks.selectAll('path').data(nodes, (d) => d.data.id);

  link.exit().remove();
  link.transition().duration(duration).attrTween('d', function (d) {
    let oldDraw = d3.select(this).attr('d');

    if (oldDraw) {
      oldDraw = oldDraw.match(/(M.*)(L.*)/);

      var oldMoveto = oldMoveto = oldDraw[1].slice(1).split(',').map(Number);
      var oldLineto = oldDraw[2].slice(1).split(',').map(Number);

      if (rootChanged && oldMoveto[1] === 0) { // Old root node
        oldMoveto = oldDraw[2].slice(1).split(',').map(Number);
        oldLineto = oldDraw[1].slice(1).split(',').map(Number);
        rootChanged = false;
      }

      if ((oldLineto !== [d.x, d.y]) && (oldMoveto !== [d.parent.x, d.parent.y])) {
        var MX = d3.interpolateNumber(oldMoveto[0], d.parent.x);
        var MY = d3.interpolateNumber(oldMoveto[1], d.parent.y);
        var LX = d3.interpolateNumber(oldLineto[0], d.x);
        var LY = d3.interpolateNumber(oldLineto[1], d.y);

        return function (t) {
          return 'M' + MX(t) + ',' + MY(t) + 'L' + LX(t) + ',' + LY(t);
        };
      }
    }
  });

  link.enter().append('path').attr('class', 'link').transition().duration(duration).attrTween('d', function (d) {
    if (d.parent) {
      var parentOldPos = oldPos[d.parent.data.id.toString()];
      var MX = d3.interpolateNumber(parentOldPos[0], d.parent.x);
      var MY = d3.interpolateNumber(parentOldPos[1], d.parent.y);
      var LX = d3.interpolateNumber(parentOldPos[0], d.x);
      var LY = d3.interpolateNumber(parentOldPos[1], d.y);

      return function (t) {
        return 'M' + MX(t) + ',' + MY(t) + 'L' + LX(t) + ',' + LY(t);
      };
    } else {
      d3.select(this).remove();
    }
  });

  const node = gNodes.selectAll('g').data(nodes, (d) => d.data.id);

  node.exit().remove();
  node.transition().duration(duration).attr('transform', function (d) {
    setTimeout(function () {
      oldPos[d.data.id.toString()] = [d.x, d.y];
    }, duration);

    return 'translate(' + d.x + ',' + d.y + ')';
  });

  const newNode = node.enter().append('g').attr('transform', function (d) {
    if (!d.parent) {
      return 'translate(' + d.x + ',' + (d.y - 30) + ')';
    } else {
      return 'translate(' + oldPos[d.parent.data.id.toString()][0] + ',' + (oldPos[d.parent.data.id.toString()][1] - 30) + ')';
    }
  }).attr('class', 'node');

  newNode.transition().duration(duration).attr('transform', function (d) {
    oldPos[d.data.id.toString()] = [d.x, d.y];
    return 'translate(' + d.x + ',' + d.y + ')';
  });

  newNode.append('circle').attr('r', 20);
  newNode.append('text').attr('class', 'text').attr('text-anchor', 'middle').attr('dy', 5).text(function (d) {
    return d.data.data;
  });
};

var handleInsert = function () {
  const number = document.getElementById('insertInput').value;

  if (number) {
    document.getElementById('insertInput').value = '';
    insertNode(parseInt(number), function () {
    });
  }

  return false;
};

var handleDelete = function () {
  const number = document.getElementById('deleteInput').value;

  if (number && data.data !== null) {
    document.getElementById('deleteInput').value = '';
    deleteNode(parseInt(number), function () {
    });
  }

  return false;
};

var handleSearch = function () {
  const number = document.getElementById('searchInput').value;

  if (number && data.data !== null) {
    document.getElementById('searchInput').value = '';
    searchNode(number);
  }

  return false;
};

var handleFill = function () {
  const number = document.getElementById('fillInput').value;

  if (number) {
    document.getElementById('fillInput').value = '';
    fillTree(number);
  }

  return false;
};

var handleClear = function () {
  location.reload();
};

$('#animationSpeed').slider({
  formatter: function (value) {
    duration = 1000 * (1 - value);
    return "";
  },
  tooltip: "hide"
});