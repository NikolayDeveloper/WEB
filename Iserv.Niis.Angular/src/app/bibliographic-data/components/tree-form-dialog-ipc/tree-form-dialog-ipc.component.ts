import { Component, OnInit, Inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatSelectionList
} from '@angular/material';
import { TreeNode } from 'primeng/components/common/treenode';
import { TreeNodeService } from '../../../shared/services/tree-node.service';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-tree-form-dialog-ipc',
  templateUrl: './tree-form-dialog-ipc.component.html',
  styleUrls: ['./tree-form-dialog-ipc.component.scss']
})
export class TreeFormDialogIpcComponent implements OnInit {
  originTree: TreeNode[] = [];
  viewTree: TreeNode[] = [];
  selectedTree: TreeNode[] = [];
  expandedNodes: string[] = [];
  labels: string[] = [];
  searchText: string;

  constructor(
    private dialogRef: MatDialogRef<TreeFormDialogIpcComponent>,
    private treeNodeService: TreeNodeService,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dictionaryService: DictionaryService
  ) {}

  ngOnInit() {
    this.setSelectedStages();
  }
  private setSelectedNode(treeNode: TreeNode, parent?: TreeNode): void {
    if (this.data.ids.find(i => i === treeNode.data) > 0) {
      treeNode.leaf = true;
      if (parent) {
        this.setExpandedNodes(parent);
      }
      this.selectedTree.push(treeNode);
    }
    if (treeNode.children) {
      for (let i = 0; i < treeNode.children.length; i++) {
        this.setSelectedNode(treeNode.children[i], treeNode);
      }
    }
  }
  private setExpandedNodes(parent: TreeNode) {
    if (this.expandedNodes.indexOf(parent.label) === -1) {
      this.expandedNodes.push(parent.label);
    }
    if (parent.parent) {
      this.setExpandedNodes(parent.parent);
    }
  }
  private setSelectedStages(): void {
    if (this.data.ids && this.data.ids.length) {
      for (let i = 0; i < this.data.tree.length; i++) {
        if (this.data.tree[i].children) {
          this.initParents(this.data.tree[i], this.data.tree[i].children);
        }
        this.setSelectedNode(this.data.tree[i]);
      }
      this.originTree = this.data.tree;
      this.treeNodeService.makeExpandedTree(
        this.originTree,
        this.expandedNodes
      );
    } else {
      this.originTree = this.data.tree;
      this.treeNodeService.makeExpandedTreeFalse(this.originTree);
    }

    this.viewTree = this.originTree;
    this.makeTreeNotSelectable(this.viewTree);
    this.expandedNodes = [];
  }
  private initParents(parent: TreeNode, children: TreeNode[]) {
    children.forEach(item => {
      item.parent = parent;
      if (item.children) {
        this.initParents(item, item.children);
      }
    });
  }
  private getDicIpcChildren(parentId: number): Observable<TreeNode[]> {
    return this.dictionaryService.getDicIpcChildren(parentId);
  }

  private makeTreeNotSelectable(tree: TreeNode[]) {
    tree.forEach(t => {
      if (!t.leaf) {
        t.selectable = false;
      }
      if (t.children) {
        this.makeTreeNotSelectable(t.children);
      }
    });
  }

  setValue(event: any): void {
    if (event.node) {
      this.data.ids.push(event.node.data);
    }
  }
  deleteValue(event: any) {
    if (!event.node) {
      return;
    }
    const index = this.data.ids.indexOf(event.node.data);
    if (index >= 0) {
      this.data.ids.splice(index, 1);
    }
  }
  onSubmit() {
    this.dialogRef.close(this.data.ids);
  }
  onCancel() {
    this.dialogRef.close(null);
  }
  searchByTree(): void {
    if (this.searchText) {
      this.dictionaryService.searchDicIpc(this.searchText).subscribe(nodes => {
        nodes.forEach(item => this.setSelectedNode(item));
        this.viewTree = nodes;
        this.makeTreeNotSelectable(this.viewTree);
      });
      this.viewTree = this.treeNodeService.searchByTree(
        this.searchText,
        this.originTree
      );
      this.makeTreeNotSelectable(this.viewTree);
    }
  }
  resetSearch(): void {
    this.searchText = '';
    this.viewTree = this.originTree;
    this.makeTreeNotSelectable(this.viewTree);
  }

  loadNode(event) {
    if (event.node) {
      if (event.node.children && event.node.children.length) {
        return;
      }
      this.getDicIpcChildren(event.node.data).subscribe((nodes: TreeNode[]) => {
        if (nodes && nodes.length) {
          event.node.children = nodes;
          if (nodes.every(n => !n.leaf)) {
            this.makeTreeNotSelectable(event.node.children);
            this.setSelectedNode(event.node);
          }
        }
      });
    }
  }
}
