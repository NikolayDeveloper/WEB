import { Injectable } from '@angular/core';
import { TreeNode } from 'primeng/components/common/treenode';

@Injectable()
export class TreeNodeService {

  constructor() { }

  public searchByTree(searchText: string, originTreeNode: TreeNode[]): TreeNode[] {
    if (!searchText) {
      return originTreeNode;
    }
    const searchTree: TreeNode[] = [];
    for (let i = 0; i < originTreeNode.length; i++) {
      const tempNode = this.getCopyTreeNode(originTreeNode[i]);
      this.setNeedNode(tempNode, searchText, searchTree);
    }
    return searchTree;
  }
  public makeExpandedTree(tree: TreeNode[], expandedNodes: string[]) {
    tree.forEach(node => {
      if (expandedNodes.indexOf(node.label) !== -1) {
        node.expanded = true;
      }
      if (node.children) {
        this.makeExpandedTree(node.children, expandedNodes);
      }
    });
  }
  public makeExpandedTreeFalse(tree: TreeNode[]) {
    tree.forEach(node => {
      node.expanded = false;
      node.partialSelected = false;
      if (node.children) {
        this.makeExpandedTreeFalse(node.children);
      }
    });
  }
  private setNeedNode(node: TreeNode, serviceName: string, searchTree: TreeNode[]): void {
    const isFound = node.children.some(item => item.label.toLocaleLowerCase().indexOf(serviceName.toLocaleLowerCase()) !== -1);
    if (isFound) {
      this.filterChildren(serviceName, node);
      node.children.forEach(c => c.parent = node);
      node.expanded = true;
      searchTree.push(node);
    } else {
      node.children.forEach(element => {
        if (element.children.length) {
          this.setNeedNode(element, serviceName, searchTree);
        }
      });
    }
  }

  private filterChildren(serviceName: string, parent: TreeNode): void {
    parent.children = parent.children.filter(item => item.label.toLocaleLowerCase().indexOf(serviceName.toLocaleLowerCase()) !== -1);
  }
  private makeAllExpandedTree(tree: TreeNode[]) {
    tree.forEach(node => {
      node.expanded = true;
      if (node.children.length) {
        this.makeAllExpandedTree(node.children);
      }
    });
  }

  private getCopyTreeNode(node: TreeNode): TreeNode {
    const newNode = Object.assign({}, node);
    newNode.children = [];
    if (node.children && node.children.length) {
      this.getCopyTreeNodeRec(newNode, node.children);
    }
    return newNode;
  }

  private getCopyTreeNodeRec(node: TreeNode, children: TreeNode[]) {
    if (children && children.length) {
      for (const child of children) {
        const tempChild = Object.assign({}, child);
        tempChild.children = [];
        this.getCopyTreeNodeRec(tempChild, child.children);
        node.children.push(tempChild);
      }
    }
  }
}
